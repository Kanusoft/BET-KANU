using BetKanu.Models.Common;


namespace BetKanu.Models
{
    public class CollaborationMedia
    {
        public int Id { get; set; }

        // Parent relationships
        public int? PartnerContributorId { get; set; }
        public PartnerContributor? PartnerContributor { get; set; }

        public int? CollaborationProjectId { get; set; }
        public CollaborationProject? CollaborationProject { get; set; }

        // File information
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? BlobName { get; set; }

        public string? ContentType { get; set; }
        public long FileSizeBytes { get; set; }

        public CollaborationMediaType MediaType { get; set; }

        // Additional information
        public string? Title { get; set; }
        public string? Caption { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }

        // Display settings
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
