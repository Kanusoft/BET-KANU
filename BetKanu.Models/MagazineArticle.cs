using BetKanu.Models.Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BetKanu.Models
{
    public class MagazineArticle
    {
        [Key]
        public int Id { get; set; }

        // Magazine relationship
        [Required]
        public int MagazineId { get; set; }

        public Magazine Magazine { get; set; } = null!;

        // General information
        [Required]
        [MaxLength(250)]
        [DisplayName("Article Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(1000)]
        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [DisplayName("Article Number")]
        public int ArticleNumber { get; set; }

        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
            = DateTime.UtcNow.Date;

        [MaxLength(500)]
        [DisplayName("Banner Image 700")]
        public string? BannerImage { get; set; }

        [DisplayName("Banner Image 350")]
        public string? BannerImage350 { get; set; }

        // Western Syriac
        [MaxLength(250)]
        [DisplayName("Western Syriac Title")]
        public string? WesternTitle { get; set; }

        [DisplayName("Western Syriac Introduction")]
        public string? WesternIntroduction { get; set; }

        [DisplayName("Western Syriac Article HTML")]
        public string? WesternBodyHtml { get; set; }

        [DisplayName("Western Syriac Credits HTML")]
        public string? WesternCreditsHtml { get; set; }

        [MaxLength(500)]
        [DisplayName("Western Syriac PDF")]
        public string? WesternPdfPath { get; set; }

        // Eastern Syriac
        [MaxLength(250)]
        [DisplayName("Eastern Syriac Title")]
        public string? EasternTitle { get; set; }

        [DisplayName("Eastern Syriac Introduction")]
        public string? EasternIntroduction { get; set; }

        [DisplayName("Eastern Syriac Article HTML")]
        public string? EasternBodyHtml { get; set; }

        [DisplayName("Eastern Syriac Credits HTML")]
        public string? EasternCreditsHtml { get; set; }

        [MaxLength(500)]
        [DisplayName("Eastern Syriac PDF")]
        public string? EasternPdfPath { get; set; }

        // Publication
        [DisplayName("Article Status")]
        public MagazineArticleStatus Status { get; set; }
            = MagazineArticleStatus.Draft;

        [DisplayName("Scheduled Publication Date")]
        public DateTime? PublishAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        // Dashboard uploads
        [NotMapped]
        [DisplayName("Banner Image 700")]
        public IFormFile? BannerImageFile { get; set; }

        [NotMapped]
        [DisplayName("Banner Image 350")]
        public IFormFile? BannerImage350File { get; set; }


        [NotMapped]
        [DisplayName("Western Syriac PDF")]
        public IFormFile? WesternPdfFile { get; set; }

        [NotMapped]
        [DisplayName("Eastern Syriac PDF")]
        public IFormFile? EasternPdfFile { get; set; }
    }
}
