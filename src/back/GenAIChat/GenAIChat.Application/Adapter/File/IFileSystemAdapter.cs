namespace GenAIChat.Application.Adapter.File
{
    public interface IFileSystemAdapter
    {
        public Task<bool> StoreDocumentFileAsync(string destinationPath, Func<FileStream, Task> copyAction, bool useUuidFolder = true);
    }
}
