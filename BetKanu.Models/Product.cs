using BetKanu.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BetKanu.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }

      
        [DisplayName("Small Image 700px")]
        public string? SmallImage { get; set; }

        [DisplayName("Cover Image 350px")]
        public string? CoverImage { get; set; }

        [Required]
        [DisplayName("Category")]
        public Category Category { get; set; }

        [DisplayName("Sub Category")]
        public string? SubCategory { get; set; }

        [DisplayName("Target Audince")]
        public Target TargetAudince { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow.Date;

        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [DisplayName("Long Description")]
        public string? LongDescription { get; set; }

        [DisplayName("Eastren Script Html")]
        public string? ScriptE { get; set; }

        [DisplayName("Westrean Script Html")]
        public string? ScriptW { get; set; }

        [DisplayName("Eastren Credits Html")]
        public string? CreditsE { get; set; }

        [DisplayName("Westrean Credits Html")]
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
        [DisplayName("Image1")]
        public string? img1 { get; set; }
        [DisplayName("Image2")]
        public string? img2 { get; set; }
        [DisplayName("Image3")]
        public string? img3 { get; set; }
        [DisplayName("Image4")]
        public string? img4 { get; set; }
        [DisplayName("Image5")]
        public string? img5 { get; set; }

        public string? Author { get; set; }
        public string? DesignedBy { get; set; }
        public string? SponsoredBY { get; set; }

        public string? Source { get; set; }
        public string? Features { get; set; }
        public string? ProductBy { get; set; }
        public string? Created { get; set; }
        public string? Link3 { get; set; }
        public string? SongsList { get; set; }
        public string? Info1 { get; set; }
        public string? Info2 { get; set; }
        public string? Info3 { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? SmallUrl { get; set; }

        [NotMapped]
        [DisplayName("Image")]
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

        [NotMapped]
        public IFormFile? WestreanPdfFile { get; set; }

        [NotMapped]
        public IFormFile? EasternPdfFile { get; set; }


        //public int? ParentId { get; set; }
        //public Product? Parent { get; set; }
    }
}
