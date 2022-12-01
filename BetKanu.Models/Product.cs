using BetKanu.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? CoverImage { get; set; }
        [Required]
        public Category Category { get; set; }
        public string? SubCategory { get; set; }
        public Target TargetAudince { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? ScriptE { get; set; }
        public string? ScriptW { get; set; }
        public string? CreditsE { get; set; }
        public string? CreditsW { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public string? VideoE { get; set; }
        public string? VideoW { get; set; }
        public int ViewsE { get; set; }
        public int ViewsW { get; set; }

        public int ParentId { get; set; }
        public Product? Parent { get; set; }
    }
}
