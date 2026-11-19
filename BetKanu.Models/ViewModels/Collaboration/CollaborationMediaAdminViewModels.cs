using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.ViewModels.Collaboration
{
    public sealed class CollaborationMediaGalleryViewModel
    {
        public int? PartnerContributorId { get; init; }
        public int? CollaborationProjectId { get; init; }
        public string ParentName { get; init; } = string.Empty;
        public CollaborationMediaType? FilterType { get; init; }
        public IReadOnlyList<CollaborationMediaDto> Items { get; init; }
            = Array.Empty<CollaborationMediaDto>();
        public string UploadAction { get; init; } = string.Empty;
        public string ReturnUrl { get; init; } = string.Empty;
    }

    public sealed class CollaborationMediaUploadFormViewModel
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

        public List<IFormFile> Files { get; set; } = new();

        public string? ReturnUrl { get; set; }
    }

    public sealed class CollaborationMediaEditViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Caption { get; set; }

        public string? Description { get; set; }

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public string? ReturnUrl { get; set; }

        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public CollaborationMediaType MediaType { get; set; }

        public CollaborationMediaUpdateDto ToUpdateDto() => new()
        {
            Title = Title,
            Caption = Caption,
            Description = Description,
            DisplayOrder = DisplayOrder,
            IsActive = IsActive
        };
    }
}
