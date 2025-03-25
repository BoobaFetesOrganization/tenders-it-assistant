using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Adapter.Api;
using TendersITAssistant.Application.Command.Common;
using TendersITAssistant.Application.Usecase.Interface;
using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Filter;
using TendersITAssistant.Domain.Project;

namespace TendersITAssistant.Application.Usecase
{
    public class DocumentApplication(IGenAiApiAdapter genAiAdapter, IMediator mediator, ILogger logger) : ApplicationBase<DocumentDomain>(mediator, logger), IApplication<DocumentDomain>
    {
        public async override Task<DocumentDomain> CreateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            base.logger.Information("application - create - args - {0}", JsonSerializer.Serialize(domain));

            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            var projectExists = await mediator.Send(new GetAllQuery<ProjectDomain>() { Filter = new PropertyEqualsFilter(nameof(ProjectDomain.Id), domain.ProjectId) }, cancellationToken);
            if (!projectExists.Any()) throw new Exception("Project not found");

            // upload files to the GenAI and add the doc if successful
            await genAiAdapter.SendFilesAsync([domain], cancellationToken);

            var result = await mediator.Send(new CreateCommand<DocumentDomain>() { Domain = domain }, cancellationToken);

            base.logger.Information("application - create - response - {0}", result is not null ? JsonSerializer.Serialize(result) : "null");

            return result;
        }

        public async override Task<bool> UpdateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            base.logger.Information("application - update - {0}", JsonSerializer.Serialize(domain));

            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            // upload files to the GenAI and update the docs if successful
            await genAiAdapter.SendFilesAsync([domain], cancellationToken);

            var response = await mediator.Send(new UpdateCommand<DocumentDomain>() { Domain = domain }, cancellationToken);

            base.logger.Information("application - update - response - {0}", response);

            return response;
        }
    }
}
