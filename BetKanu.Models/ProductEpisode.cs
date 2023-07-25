using BetKanu.Models.Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BetKanu.Models
{
    public class ProductEpisode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Views { get; set; }

        [DisplayName("Eastrean Video Linke")]
        public string? VideoE { get; set; }

        [DisplayName("Westrean Video Linke")]
        public string? VideoW { get; set; }

        [DisplayName("Eastrean Image")]
        public string? ImageE { get; set; }

        [DisplayName("Westren Image")]
        public string? ImageW { get; set; }

        public Language? Status { get; set; }

        public DateTime ReleaseDate { get; set; } = DateTime.Now.Date;
        public int ProductId { get; set; }
        public Product? Product { get; set; }



        [NotMapped]
        public IFormFile? EastrenImageFile { get; set; }

        [NotMapped]
        public IFormFile? WestreanImageFile { get; set; }
    }
}
