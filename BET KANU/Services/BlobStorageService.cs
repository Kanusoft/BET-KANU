using BetKanu.Models.DTOs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BET_KANU.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string? _storageConnectionString;
        private readonly string? _storageContainerName;

        public BlobStorageService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("BlobContainerName");
        }

        public async Task UploadBlobImageAsync(IFormFile files)
        {
            try
            {
                byte[] dataFiles;
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_storageContainerName);
                await cloudBlobContainer.CreateIfNotExistsAsync();

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await cloudBlobContainer.SetPermissionsAsync(permissions);
                await using (var target = new MemoryStream())
                {
                    files.CopyTo(target);
                    dataFiles = target.ToArray();
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference("products/" + files.FileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);
            }
            catch (Exception)
            {
                throw new Exception("Image Upload Failed");
            }
        }

        public async Task UploadBlobFileAsync(IFormFile files)
        {
            try
            {
                byte[] dataFiles;
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_storageContainerName);
                await cloudBlobContainer.CreateIfNotExistsAsync();

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await cloudBlobContainer.SetPermissionsAsync(permissions);
                await using (var target = new MemoryStream())
                {
                    files.CopyTo(target);
                    dataFiles = target.ToArray();
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference("PDF/" + files.FileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);
            }
            catch (Exception)
            {
                throw new Exception("File Upload Failed");
            }
        }

        public async Task DeleteDocumentAsync(string blobName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(blobName))
                    return;

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_storageContainerName);
                var blob = cloudBlobContainer.GetBlobReference(blobName.TrimStart('/'));
                await blob.DeleteIfExistsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BlobUploadResult> UploadAsync(
            Stream content,
            string blobName,
            string? contentType = null,
            CancellationToken cancellationToken = default)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name is required.", nameof(blobName));

            var normalizedName = blobName.Replace('\\', '/').TrimStart('/');

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_storageContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(normalizedName);
            if (!string.IsNullOrWhiteSpace(contentType))
                cloudBlockBlob.Properties.ContentType = contentType;

            if (content.CanSeek)
                content.Position = 0;

            await cloudBlockBlob.UploadFromStreamAsync(content);

            return new BlobUploadResult
            {
                BlobName = normalizedName,
                Url = cloudBlockBlob.Uri.AbsoluteUri
            };
        }

        public string GetPublicUrl(string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                return string.Empty;

            var normalizedName = blobName.Replace('\\', '/').TrimStart('/');
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_storageContainerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(normalizedName);
            return cloudBlockBlob.Uri.AbsoluteUri;
        }

        public async Task<bool> ExistsAsync(string blobName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                return false;

            var normalizedName = blobName.Replace('\\', '/').TrimStart('/');
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_storageContainerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(normalizedName);
            return await cloudBlockBlob.ExistsAsync();
        }
    }
}
