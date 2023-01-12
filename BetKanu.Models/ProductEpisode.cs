using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string? Status { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [NotMapped]
        public IFormFile? imgUrl { get; set; }

        [NotMapped]
        public IFormFile? imgUrl2 { get; set; }
    }
}
