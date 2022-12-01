using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string? VideoE { get; set; }
        public string? VideoW { get; set; }
        public string? ImageE { get; set; }
        public string? ImageW { get; set; }
        public string? Status { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
