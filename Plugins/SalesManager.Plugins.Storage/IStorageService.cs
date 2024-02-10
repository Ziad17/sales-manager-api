using Microsoft.AspNetCore.Http;

namespace SalesManager.Plugins.Storage
{
    public interface IStorageService
    {
        Task<string> UploadAsync(IFormFile file, string destination, CancellationToken cancellationToken);

        Task<string> UploadAsync(byte[] file, string fileName, string destination = null,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(string fileUrl, CancellationToken cancellationToken);
    }
}
