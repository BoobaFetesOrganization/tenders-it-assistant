using AutoMapper;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal partial class DocumentRepository : GenericRepository<DocumentDomain, DocumentEntity>
    {
        private const string ContainerName = "Documents";

        [GeneratedRegex("[^a-zA-Z0-9\\-]")]
        private static partial Regex BlobNameRegex();

        private readonly BlobContainerClient blobContainer;
        private readonly IRepositoryAdapter<DocumentMetadataDomain> metadataRepository;
        private static string GetBlobNameFromId(string id) => BlobNameRegex().Replace(id, "--");

        public DocumentRepository(BlobServiceClient blobService, TableServiceClient tableService, IMapper mapper, IRepositoryAdapter<DocumentMetadataDomain> metadataRepository)
            : base(tableService, ContainerName, mapper)
        {
            blobContainer = blobService.GetBlobContainerClient(ContainerName.ToLower());
            blobContainer.CreateIfNotExistsAsync();
            this.metadataRepository = metadataRepository;
        }

        public async override Task<DocumentDomain> AddAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<DocumentDomain>(domain);
            clone.Id = TableStorageTools.GetNewId();
            clone.Metadata.DocumentId = clone.Id;

            // create related entity metadata
            clone.Metadata = await metadataRepository.AddAsync(clone.Metadata, cancellationToken);

            // create the blob in the container
            if (clone.Content.Length > 0)
            {
                try
                {
                    var blobClient = blobContainer.GetBlobClient(GetBlobNameFromId(clone.Id));
                    await blobClient.UploadAsync(new MemoryStream(clone.Content), true, cancellationToken);
                }
                catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NoContent)
                {
                    // faudrait voir si on peut ajouter des traces
                }
                catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
                {
                    // faudrait voir si on peut ajouter des tracesreturn null;
                }
                catch (Exception) // on sait jamais, faudra revoir ca un jour ....
                {
                    // faudrait voir si on peut ajouter des traces
                }
            }

            // create document
            return await base.AddAsync(clone, cancellationToken);
        }

        public async override Task<bool> DeleteAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            // update or delete the blob in the container
            try
            {
                var blobClient = blobContainer.GetBlobClient(GetBlobNameFromId(domain.Id));
                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, null, cancellationToken);

                // cascading deletion of all related entities
                if (!await metadataRepository.DeleteAsync(domain.Metadata, cancellationToken)) return false;
                return await base.DeleteAsync(domain, cancellationToken);
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NoContent)
            {
                return false;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception) // on sait jamais, faudra revoir ca un jour ....
            {
                return false;
            }
        }

        public async override Task<DocumentDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetByIdAsync(id, cancellationToken);
            if (result is null) return null;

            //cascading loading of all related entities 
            result.Metadata = (await metadataRepository.GetAllAsync(new PropertyEqualsFilter(nameof(DocumentMetadataEntity.DocumentId), id), cancellationToken)).Single();

            var blobClient = blobContainer.GetBlobClient(GetBlobNameFromId(id));
            var downloadResult = await blobClient.DownloadContentAsync(cancellationToken);
            if (downloadResult.Value.Content is not null)
                result.Content = downloadResult.Value.Content.ToMemory().ToArray();

            return result;
        }

        public async override Task<bool> UpdateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            try
            {
                // update or delete the blob in the container
                var blobClient = blobContainer.GetBlobClient(domain.Id);
                if (domain.Content.Length > 0)
                    await blobClient.UploadAsync(new MemoryStream(domain.Content), true, cancellationToken);
                else
                    await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, null, cancellationToken);

                // we separate the tasks to not lose all the data if the file deletion task fails
                List<Task<bool>> actions = [
                    base.UpdateAsync(domain, cancellationToken),
                    metadataRepository.UpdateAsync(domain.Metadata, cancellationToken),
                ];

                var actionResults = await Task.WhenAll(actions);
                return actionResults.All(x => x);
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NoContent)
            {
                return false;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception) // on sait jamais, faudra revoir ca un jour ....
            {
                return false;
            }
        }
    }
}