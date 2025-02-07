using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAIChat.Application.Adapter.File
{
    public interface IFileSystemAdapter
    {
        public Task<bool> StoreDocumentFileAsync(string destinationPath, Func<FileStream, Task> copyAction, bool useUuidFolder = true);
    }
}
