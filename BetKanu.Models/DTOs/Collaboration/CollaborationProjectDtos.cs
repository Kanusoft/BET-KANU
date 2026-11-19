using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.DTOs.Collaboration
{
    public sealed class CollaborationProjectCreateDto
    {
        [Required]
        public int PartnerContributorId { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        [StringLength(1000)]
        public string? ProjectLink { get; set; }

        [StringLength(150)]
        public string? ProjectType { get; set; }

        public int? BookId { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        public int? ContentPageId { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }
    }

    public sealed class CollaborationProjectUpdateDto
    {
        [Required]
        public int PartnerContributorId { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        [StringLength(1000)]
        public string? ProjectLink { get; set; }

        [StringLength(150)]
        public string? ProjectType { get; set; }

        public int? BookId { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        public int? ContentPageId { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }
    }

    public sealed class CollaborationProjectFilterDto
    {
        public string? Search { get; set; }
        public int? PartnerContributorId { get; set; }
        public int? Year { get; set; }
        public string? ProjectType { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public sealed class CollaborationProjectListDto
    {
        public int Id { get; init; }
        public int PartnerContributorId { get; init; }
        public string PartnerContributorName { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string? LogoUrl { get; init; }
        public string? ShortDescription { get; init; }
        public string? ProjectType { get; init; }
        public int? Year { get; init; }
        public bool IsActive { get; init; }
        public int DisplayOrder { get; init; }
        public int MediaCount { get; init; }
    }

    public sealed class CollaborationProjectDetailsDto
    {
        public int Id { get; init; }
        public int PartnerContributorId { get; init; }
        public string PartnerContributorName { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string? LogoUrl { get; init; }
        public string? ShortDescription { get; init; }
        public string? LongDescription { get; init; }
        public string? ProjectLink { get; init; }
        public string? ProjectType { get; init; }
        public int? BookId { get; init; }
        public int? Year { get; init; }
        public int? ContentPageId { get; init; }
        public bool IsActive { get; init; }
        public int DisplayOrder { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public IReadOnlyList<CollaborationMediaDto> Media { get; init; }
            = Array.Empty<CollaborationMediaDto>();
    }
}
