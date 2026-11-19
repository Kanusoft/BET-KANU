using Microsoft.AspNetCore.Http;
using System.Text;

namespace BetKanu.Models.Common
{
    public static class CollaborationFileValidator
    {
        public sealed record FileValidationResult(
            bool IsValid,
            CollaborationMediaType MediaType,
            string SanitizedFileName,
            string Extension,
            string? Error);

        public static FileValidationResult Validate(
            IFormFile file,
            CollaborationUploadOptions options,
            bool imagesOnly = false)
        {
            if (file == null || file.Length <= 0)
                return Fail("The uploaded file is empty.");

            var originalName = Path.GetFileName(file.FileName ?? string.Empty);
            if (string.IsNullOrWhiteSpace(originalName) ||
                originalName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                originalName.Contains("..", StringComparison.Ordinal))
            {
                return Fail("The file name is invalid.");
            }

            var extension = Path.GetExtension(originalName).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension))
                return Fail("The file extension is required.");

            if (!TryResolveMediaType(extension, options, out var mediaType))
                return Fail($"The file type '{extension}' is not allowed.");

            if (imagesOnly && mediaType != CollaborationMediaType.Image)
                return Fail("Only image files (JPG, JPEG, PNG, WebP) are allowed for logos.");

            var maxBytes = GetMaxBytes(mediaType, options);
            if (file.Length > maxBytes)
                return Fail($"The file exceeds the maximum allowed size of {maxBytes / (1024 * 1024)} MB.");

            var contentType = (file.ContentType ?? string.Empty).Trim().ToLowerInvariant();
            if (!IsMimeAllowed(mediaType, contentType, options))
                return Fail($"The content type '{file.ContentType}' is not allowed for this file.");

            if (!LooksLikeValidContent(file, extension, mediaType))
                return Fail("The file content does not match the declared file type.");

            var sanitized = SanitizeFileName(originalName);
            return new FileValidationResult(true, mediaType, sanitized, extension, null);
        }

        private static FileValidationResult Fail(string error) =>
            new(false, CollaborationMediaType.OtherFile, string.Empty, string.Empty, error);

        private static bool TryResolveMediaType(
            string extension,
            CollaborationUploadOptions options,
            out CollaborationMediaType mediaType)
        {
            if (options.ImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                mediaType = CollaborationMediaType.Image;
                return true;
            }

            if (options.VideoExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                mediaType = CollaborationMediaType.Video;
                return true;
            }

            if (options.PdfExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                mediaType = CollaborationMediaType.Pdf;
                return true;
            }

            if (options.DocumentExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                mediaType = CollaborationMediaType.Document;
                return true;
            }

            if (options.OtherFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                mediaType = CollaborationMediaType.OtherFile;
                return true;
            }

            mediaType = CollaborationMediaType.OtherFile;
            return false;
        }

        private static long GetMaxBytes(CollaborationMediaType type, CollaborationUploadOptions options) =>
            type switch
            {
                CollaborationMediaType.Image => options.MaxImageBytes,
                CollaborationMediaType.Video => options.MaxVideoBytes,
                CollaborationMediaType.Pdf => options.MaxPdfBytes,
                CollaborationMediaType.Document => options.MaxDocumentBytes,
                _ => options.MaxOtherBytes
            };

        private static bool IsMimeAllowed(
            CollaborationMediaType type,
            string contentType,
            CollaborationUploadOptions options)
        {
            if (string.IsNullOrWhiteSpace(contentType))
                return false;

            var allowed = type switch
            {
                CollaborationMediaType.Image => options.ImageMimeTypes,
                CollaborationMediaType.Video => options.VideoMimeTypes,
                CollaborationMediaType.Pdf => options.PdfMimeTypes,
                CollaborationMediaType.Document => options.DocumentMimeTypes,
                _ => options.OtherFileMimeTypes
            };

            return allowed.Contains(contentType, StringComparer.OrdinalIgnoreCase);
        }

        private static bool LooksLikeValidContent(IFormFile file, string extension, CollaborationMediaType mediaType)
        {
            try
            {
                using var stream = file.OpenReadStream();
                Span<byte> header = stackalloc byte[16];
                var read = stream.Read(header);
                if (read < 4)
                    return false;

                return mediaType switch
                {
                    CollaborationMediaType.Image when extension is ".jpg" or ".jpeg"
                        => header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
                    CollaborationMediaType.Image when extension == ".png"
                        => header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47,
                    CollaborationMediaType.Image when extension == ".webp"
                        => read >= 12
                           && Encoding.ASCII.GetString(header[..4]) == "RIFF"
                           && Encoding.ASCII.GetString(header.Slice(8, 4)) == "WEBP",
                    CollaborationMediaType.Pdf
                        => Encoding.ASCII.GetString(header[..4]) == "%PDF",
                    CollaborationMediaType.Video when extension == ".mp4"
                        => read >= 8 && (
                            Encoding.ASCII.GetString(header.Slice(4, 4)) is "ftyp" or "moov" or "mdat"),
                    CollaborationMediaType.Video when extension == ".webm"
                        => header[0] == 0x1A && header[1] == 0x45 && header[2] == 0xDF && header[3] == 0xA3,
                    CollaborationMediaType.Document when extension == ".txt"
                        => true,
                    CollaborationMediaType.Document
                        => header[0] == 0x50 && header[1] == 0x4B, // ZIP-based Office Open XML
                    CollaborationMediaType.OtherFile when extension == ".zip"
                        => header[0] == 0x50 && header[1] == 0x4B,
                    CollaborationMediaType.OtherFile when extension == ".csv"
                        => true,
                    _ => true
                };
            }
            catch
            {
                return false;
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            var sb = new StringBuilder(name.Length);
            foreach (var ch in name)
            {
                if (char.IsLetterOrDigit(ch) || ch is '-' or '_' or '.')
                    sb.Append(ch);
                else
                    sb.Append('-');
            }

            var cleaned = sb.ToString().Trim('-');
            if (string.IsNullOrWhiteSpace(cleaned))
                cleaned = "file";

            if (cleaned.Length > 80)
                cleaned = cleaned[..80];

            return cleaned + ext;
        }
    }
}
