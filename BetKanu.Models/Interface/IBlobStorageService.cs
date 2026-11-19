using BetKanu.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace BET_KANU.Services
{
    public interface IBlobStorageService
    {
        Task UploadBlobImageAsync(IFormFile files);
        Task UploadBlobFileAsync(IFormFile files);
        Task DeleteDocumentAsync(string blobName);

        /// <summary>
        /// Uploads content to an explicit blob path and returns the blob name plus public URL.
        /// Added for collaboration logos/media without replacing existing upload helpers.
        /// </summary>
        Task<BlobUploadResult> UploadAsync(
            Stream content,
            string blobName,
            string? contentType = null,
            CancellationToken cancellationToken = default);

        string GetPublicUrl(string blobName);

        Task<bool> ExistsAsync(string blobName, CancellationToken cancellationToken = default);
    }
}
