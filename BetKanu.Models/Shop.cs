using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BetKanu.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Sale Title")]
        public string? Name { get; set; }
        [DisplayName("Image")]
        public string? ImageUrl { get; set; }

        [DisplayName("Price")]
        public int Price { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow.Date;

        [NotMapped]
        public IFormFile? FileUrl { get; set; }
    }
}
