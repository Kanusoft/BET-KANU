
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetKanu.Models.ViewModels
{
    public class MagazineFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Magazine title is required.")]
        [StringLength(
            200,
            MinimumLength = 2,
            ErrorMessage = "Magazine title must be between 2 and 200 characters.")]
        [DisplayName("Magazine Title")]
        public string Title { get; set; } = string.Empty;


        [StringLength(
            500,
            ErrorMessage = "Short description cannot exceed 500 characters.")]
        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [DisplayName("Long Description")]
        public string? LongDescription { get; set; }

        [Range(
            1900,
            3000,
            ErrorMessage = "Please enter a valid magazine year.")]
        [DisplayName("Magazine Year")]
        public int Year { get; set; } = DateTime.UtcNow.Year;

        [Range(
            0,
            int.MaxValue,
            ErrorMessage = "Display order cannot be negative.")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

        [DisplayName("Published")]
        public bool IsPublished { get; set; }

        [DisplayName("Publication Date")]
        [DataType(DataType.DateTime)]
        public DateTime? PublishedAtUtc { get; set; }

        public string? ExistingCoverImage { get; set; }

        [DisplayName("Cover Image")]
        public IFormFile? CoverImageFile { get; set; }

        public string? ExistingCoverImage350 { get; set; }

        [DisplayName("Cover Image 350")]
        public IFormFile? CoverImage350File { get; set; }

        public bool IsEdit => Id > 0;
    }
}
