namespace BET_KANU.Services
{
    public interface IBlobStorageService
    {
        Task UploadBlobFileAsync(IFormFile files);
        Task DeleteDocumentAsync(string blobName);
    }
}
