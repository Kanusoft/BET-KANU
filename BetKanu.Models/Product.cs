using BetKanu.Models.Common;
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
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }

      
        [DisplayName("Small Image 700")]
        public string? SmallImage { get; set; }

        [DisplayName("Cover Image 350")]
        public string? CoverImage { get; set; }

        [Required]
        [DisplayName("Category")]
        public Category Category { get; set; }

        [DisplayName("Sub Category")]
        public string? SubCategory { get; set; }

        [DisplayName("Target Audince")]
        public Target TargetAudince { get; set; }

        [DisplayName("Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [DisplayName("Long Description")]
        public string? LongDescription { get; set; }

        [DisplayName("Eastren Script")]
        public string? ScriptE { get; set; }

        [DisplayName("Westrean Script")]
        public string? ScriptW { get; set; }

        [DisplayName("Eastren Credits")]
        public string? CreditsE { get; set; }

        [DisplayName("Westrean Credits")]
        public string? CreditsW { get; set; }

        public string? Link1 { get; set; }
        public string? Link2 { get; set; }

        [DisplayName("Westrean Pdf Link")]
        public string? Wpdf { get; set; }

        [DisplayName("Eastren Pdf Link")]
        public string? Epdf { get; set; }

        [DisplayName("Eastren Video Link")]
        public string? VideoE { get; set; }

        [DisplayName("Westrean Video Link")]
        public string? VideoW { get; set; }

        [DisplayName("Eastren Video Views")]
        public int ViewsE { get; set; }

        [DisplayName("Westrean Video Views")]
        public int ViewsW { get; set; }

        public string? img1 { get; set; }
        public string? img2 { get; set; }
        public string? img3 { get; set; }
        public string? img4 { get; set; }
        public string? img5 { get; set; }

        [NotMapped]
        public IFormFile? SmallUrl { get; set; }

        [NotMapped]
        public IFormFile? CoverUrl { get; set; }

        [NotMapped]
        public IFormFile? imgUrl { get; set; }

        [NotMapped]
        public IFormFile? imgUrl2 { get; set; }

        [NotMapped]
        public IFormFile? imgUrl3 { get; set; }

        [NotMapped]
        public IFormFile? imgUrl4 { get; set; }

        [NotMapped]
        public IFormFile? imgUrl5 { get; set; }

       

        //public int ParentId { get; set; }
        //public Product Parent { get; set; }
    }
}
