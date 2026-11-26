using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.ViewModels.Collaboration
{
    public sealed class CollaborationContentOptionItem
    {
        public string Value { get; init; } = string.Empty;
        public string Text { get; init; } = string.Empty;
        public string Group { get; init; } = string.Empty;
    }

    public sealed class CollaborationProjectFormViewModel
    {
        public int? Id { get; set; }

        [Required]
        public int PartnerContributorId { get; set; }

        public string PartnerContributorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project title is required.")]
        [StringLength(250)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        [StringLength(1000)]
        public string? ProjectLink { get; set; }

        [StringLength(150)]
        public string? ProjectType { get; set; }

        [Display(Name = "Associated BET KANU Content")]
        public string? ContentSelection { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        public int? ContentPageId { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }

        public string? ExistingLogoUrl { get; set; }

        public IFormFile? LogoFile { get; set; }

        public string ActiveTab { get; set; } = "information";

        public IEnumerable<CollaborationContentOptionItem> ContentOptions { get; set; }
            = Enumerable.Empty<CollaborationContentOptionItem>();

        public IReadOnlyList<CollaborationMediaDto> Media { get; set; }
            = Array.Empty<CollaborationMediaDto>();

        public static CollaborationProjectFormViewModel FromDetails(
            CollaborationProjectDetailsDto dto,
            IEnumerable<CollaborationContentOptionItem> contentOptions) => new()
        {
            Id = dto.Id,
            PartnerContributorId = dto.PartnerContributorId,
            PartnerContributorName = dto.PartnerContributorName,
            Title = dto.Title,
            ShortDescription = dto.ShortDescription,
            LongDescription = dto.LongDescription,
            ProjectLink = dto.ProjectLink,
            ProjectType = dto.ProjectType,
            ContentSelection = CollaborationContentSelection.Format(dto.ContentType, dto.ContentId),
            Year = dto.Year,
            ContentPageId = dto.ContentPageId,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            ExistingLogoUrl = dto.LogoUrl,
            ContentOptions = contentOptions,
            Media = dto.Media
        };

        public bool TryApplyContentSelection(
            out CollaborationContentType? contentType,
            out int? contentId,
            out string? error)
            => CollaborationContentSelection.TryParse(ContentSelection, out contentType, out contentId, out error);

        public CollaborationProjectCreateDto ToCreateDto(
            CollaborationContentType? contentType,
            int? contentId) => new()
        {
            PartnerContributorId = PartnerContributorId,
            Title = Title,
            ShortDescription = ShortDescription,
            LongDescription = LongDescription,
            ProjectLink = ProjectLink,
            ProjectType = ProjectType,
            ContentType = contentType,
            ContentId = contentId,
            Year = Year,
            ContentPageId = ContentPageId,
            IsActive = IsActive,
            DisplayOrder = DisplayOrder
        };

        public CollaborationProjectUpdateDto ToUpdateDto(
            CollaborationContentType? contentType,
            int? contentId) => new()
        {
            PartnerContributorId = PartnerContributorId,
            Title = Title,
            ShortDescription = ShortDescription,
            LongDescription = LongDescription,
            ProjectLink = ProjectLink,
            ProjectType = ProjectType,
            ContentType = contentType,
            ContentId = contentId,
            Year = Year,
            ContentPageId = ContentPageId,
            IsActive = IsActive,
            DisplayOrder = DisplayOrder
        };
    }
}
