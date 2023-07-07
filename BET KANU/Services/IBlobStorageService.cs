namespace BET_KANU.Services
{
    public interface IBlobStorageService
    {
        Task UploadBlobImageAsync(IFormFile files);
        Task UploadBlobFileAsync(IFormFile files);
        Task DeleteDocumentAsync(string blobName);
    }
}
