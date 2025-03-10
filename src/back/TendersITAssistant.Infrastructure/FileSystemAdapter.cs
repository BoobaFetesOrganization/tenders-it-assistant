using TendersITAssistant.Application.Adapter.File;

namespace TendersITAssistant.Infrastructure
{
    public class FileSystemAdapter : IFileSystemAdapter
    {

        public async Task<bool> StoreDocumentFileAsync(string destinationPath, Func<FileStream, Task> copyAction, bool useUuidFolder = true)
        {
            try
            {
                using (var stream = new FileStream(destinationPath, FileMode.Create))
                {
                    await copyAction(stream);
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Error while storing file", ex);
            }
            return true;
        }
    }
}
