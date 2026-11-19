using BetKanu.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.DTOs.Collaboration
{
    public sealed class PartnerContributorCreateDto
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        [StringLength(1000)]
        public string? WebsiteUrl { get; set; }

        [Required]
        public CollaborationCategory Category { get; set; }

        [StringLength(150)]
        public string? PartnerType { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }
    }

    public sealed class PartnerContributorUpdateDto
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        [StringLength(1000)]
        public string? WebsiteUrl { get; set; }

        [Required]
        public CollaborationCategory Category { get; set; }

        [StringLength(150)]
        public string? PartnerType { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }
    }

    public sealed class PartnerContributorFilterDto
    {
        public string? Search { get; set; }
        public CollaborationCategory? Category { get; set; }
        public bool? IsActive { get; set; }
        public int? Year { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public sealed class PartnerContributorListDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? LogoUrl { get; init; }
        public string? ShortDescription { get; init; }
        public CollaborationCategory Category { get; init; }
        public string? PartnerType { get; init; }
        public int? Year { get; init; }
        public bool IsActive { get; init; }
        public int DisplayOrder { get; init; }
        public int ProjectCount { get; init; }
        public int MediaCount { get; init; }
    }

    public sealed class PartnerContributorDetailsDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? LogoUrl { get; init; }
        public string? ShortDescription { get; init; }
        public string? LongDescription { get; init; }
        public string? WebsiteUrl { get; init; }
        public CollaborationCategory Category { get; init; }
        public string? PartnerType { get; init; }
        public int? Year { get; init; }
        public bool IsActive { get; init; }
        public int DisplayOrder { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public IReadOnlyList<CollaborationProjectListDto> Projects { get; init; }
            = Array.Empty<CollaborationProjectListDto>();
        public IReadOnlyList<CollaborationMediaDto> Media { get; init; }
            = Array.Empty<CollaborationMediaDto>();
    }
}
