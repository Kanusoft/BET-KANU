

using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetKanu.Models
{
    public class Magazine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [DisplayName("Magazine Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(500)]
        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [DisplayName("Long Description")]
        public string? LongDescription { get; set; }

        [DisplayName("Magazine Year")]
        public int Year { get; set; }

        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

        [MaxLength(500)]
        [DisplayName("Cover Image 700")]
        public string? CoverImage { get; set; }

        [DisplayName("Cover Image 350")]
        public string? CoverImage350 { get; set; }

        [DisplayName("Published")]
        public bool IsPublished { get; set; }

        [DisplayName("Publication Date")]
        public DateTime? PublishedAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        /*
         * Navigation properties
         */

        public ICollection<MagazineArticle> Articles { get; set; }
            = new List<MagazineArticle>();

        /*
         * Uploaded files used by the dashboard.
         * These properties are not saved directly in the database.
         */

        [NotMapped]
        [DisplayName("Cover Image 700")]
        public IFormFile? CoverImageFile { get; set; }

        [NotMapped]
        [DisplayName("Cover Image 350")]
        public IFormFile? CoverImage350File { get; set; }
    }
}
