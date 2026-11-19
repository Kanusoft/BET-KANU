using BetKanu.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.DTOs.Collaboration
{
    public sealed class CollaborationMediaUploadDto
    {
        public int? PartnerContributorId { get; set; }
        public int? CollaborationProjectId { get; set; }

        [StringLength(250)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Caption { get; set; }

        public string? Description { get; set; }

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Optional hint. Actual type is inferred from validated file content/extension.
        /// </summary>
        public CollaborationMediaType? MediaTypeHint { get; set; }
    }

    public sealed class CollaborationMediaUpdateDto
    {
        [StringLength(250)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Caption { get; set; }

        public string? Description { get; set; }

        [StringLength(1500)]
        public string? ThumbnailUrl { get; set; }

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public sealed class CollaborationMediaDto
    {
        public int Id { get; init; }
        public int? PartnerContributorId { get; init; }
        public int? CollaborationProjectId { get; init; }
        public string FileName { get; init; } = string.Empty;
        public string FileUrl { get; init; } = string.Empty;
        public string? BlobName { get; init; }
        public string? ContentType { get; init; }
        public long FileSizeBytes { get; init; }
        public CollaborationMediaType MediaType { get; init; }
        public string? Title { get; init; }
        public string? Caption { get; init; }
        public string? Description { get; init; }
        public string? ThumbnailUrl { get; init; }
        public int DisplayOrder { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
