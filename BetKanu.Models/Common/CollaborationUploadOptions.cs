namespace BetKanu.Models.Common
{
    public sealed class CollaborationUploadOptions
    {
        public const string SectionName = "CollaborationUpload";

        public long MaxImageBytes { get; set; } = 5 * 1024 * 1024;
        public long MaxVideoBytes { get; set; } = 100 * 1024 * 1024;
        public long MaxPdfBytes { get; set; } = 20 * 1024 * 1024;
        public long MaxDocumentBytes { get; set; } = 20 * 1024 * 1024;
        public long MaxOtherBytes { get; set; } = 10 * 1024 * 1024;

        public string[] ImageExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".webp" };
        public string[] VideoExtensions { get; set; } = { ".mp4", ".webm" };
        public string[] PdfExtensions { get; set; } = { ".pdf" };
        public string[] DocumentExtensions { get; set; } = { ".docx", ".xlsx", ".pptx", ".txt" };
        public string[] OtherFileExtensions { get; set; } = { ".zip", ".csv" };

        public string[] ImageMimeTypes { get; set; } =
        {
            "image/jpeg", "image/png", "image/webp", "application/octet-stream"
        };

        public string[] VideoMimeTypes { get; set; } =
        {
            "video/mp4", "video/webm", "application/octet-stream"
        };

        public string[] PdfMimeTypes { get; set; } =
        {
            "application/pdf", "application/octet-stream"
        };

        public string[] DocumentMimeTypes { get; set; } =
        {
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "text/plain",
            "application/octet-stream"
        };

        public string[] OtherFileMimeTypes { get; set; } =
        {
            "application/zip",
            "application/x-zip-compressed",
            "text/csv",
            "application/csv",
            "application/octet-stream"
        };
    }
}
