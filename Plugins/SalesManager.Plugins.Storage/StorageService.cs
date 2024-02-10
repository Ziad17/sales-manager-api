using Microsoft.AspNetCore.Http;

namespace SalesManager.Plugins.Storage
{
    public class StorageService : IStorageService
    {
        public async Task<string> UploadAsync(IFormFile file, string destination, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(byte[] file, string fileName, string destination = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string fileUrl, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
