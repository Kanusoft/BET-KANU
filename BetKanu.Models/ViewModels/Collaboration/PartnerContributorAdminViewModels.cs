using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.ViewModels.Collaboration
{
    public sealed class PartnerContributorIndexViewModel
    {
        public string? Search { get; init; }
        public CollaborationCategory? Category { get; init; }
        public bool? IsActive { get; init; }
        public int? Year { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public PagedResult<PartnerContributorListDto> Results { get; init; } = new();
    }

    public sealed class PartnerContributorFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Organization name is required.")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        [StringLength(1000)]
        public string? WebsiteUrl { get; set; }

        [Required]
        public CollaborationCategory Category { get; set; } = CollaborationCategory.Partner;

        [StringLength(150)]
        public string? PartnerType { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }

        public string? ExistingLogoUrl { get; set; }

        public IFormFile? LogoFile { get; set; }

        public string ActiveTab { get; set; } = "information";

        public IReadOnlyList<CollaborationProjectListDto> Projects { get; set; }
            = Array.Empty<CollaborationProjectListDto>();

        public IReadOnlyList<CollaborationMediaDto> Media { get; set; }
            = Array.Empty<CollaborationMediaDto>();

        public static PartnerContributorFormViewModel FromDetails(PartnerContributorDetailsDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            ShortDescription = dto.ShortDescription,
            LongDescription = dto.LongDescription,
            WebsiteUrl = dto.WebsiteUrl,
            Category = dto.Category,
            PartnerType = dto.PartnerType,
            Year = dto.Year,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            ExistingLogoUrl = dto.LogoUrl,
            Projects = dto.Projects,
            Media = dto.Media
        };

        public PartnerContributorCreateDto ToCreateDto() => new()
        {
            Name = Name,
            ShortDescription = ShortDescription,
            LongDescription = LongDescription,
            WebsiteUrl = WebsiteUrl,
            Category = Category,
            PartnerType = PartnerType,
            Year = Year,
            IsActive = IsActive,
            DisplayOrder = DisplayOrder
        };

        public PartnerContributorUpdateDto ToUpdateDto() => new()
        {
            Name = Name,
            ShortDescription = ShortDescription,
            LongDescription = LongDescription,
            WebsiteUrl = WebsiteUrl,
            Category = Category,
            PartnerType = PartnerType,
            Year = Year,
            IsActive = IsActive,
            DisplayOrder = DisplayOrder
        };
    }

    public sealed class PartnerContributorDetailsViewModel
    {
        public PartnerContributorDetailsDto Item { get; init; } = null!;
    }
}
