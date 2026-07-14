using BetKanu.Models.Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.ViewModels.MagazineArticles
{
    public class MagazineArticleFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public int MagazineId { get; set; }

        public string MagazineTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Article title is required.")]
        [StringLength(250, MinimumLength = 2)]
        [DisplayName("Article Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [Range(1, int.MaxValue)]
        [DisplayName("Article Number")]
        public int ArticleNumber { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        public string? ExistingBannerImage { get; set; }

        [DisplayName("Banner Image")]
        public IFormFile? BannerImageFile { get; set; }

        [StringLength(250)]
        [DisplayName("Western Syriac Title")]
        public string? WesternTitle { get; set; }

        [DisplayName("Western Syriac Introduction")]
        public string? WesternIntroduction { get; set; }

        [DisplayName("Western Syriac Article HTML")]
        public string? WesternBodyHtml { get; set; }

        [DisplayName("Western Syriac Credits HTML")]
        public string? WesternCreditsHtml { get; set; }

        public string? ExistingWesternPdfPath { get; set; }

        [DisplayName("Western Syriac PDF")]
        public IFormFile? WesternPdfFile { get; set; }

        [StringLength(250)]
        [DisplayName("Eastern Syriac Title")]
        public string? EasternTitle { get; set; }

        [DisplayName("Eastern Syriac Introduction")]
        public string? EasternIntroduction { get; set; }

        [DisplayName("Eastern Syriac Article HTML")]
        public string? EasternBodyHtml { get; set; }

        [DisplayName("Eastern Syriac Credits HTML")]
        public string? EasternCreditsHtml { get; set; }

        public string? ExistingEasternPdfPath { get; set; }

        [DisplayName("Eastern Syriac PDF")]
        public IFormFile? EasternPdfFile { get; set; }

        [DisplayName("Article Status")]
        public MagazineArticleStatus Status { get; set; }
            = MagazineArticleStatus.Draft;

        [DisplayName("Publish Date")]
        public DateTime? PublishAtUtc { get; set; }

        public bool IsEdit => Id > 0;
    }
}
