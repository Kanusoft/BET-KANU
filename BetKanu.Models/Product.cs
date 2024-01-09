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

      
        [DisplayName("Home Page Image (700px)")]
        public string? SmallImage { get; set; }

        [DisplayName("Cover Image (350px)")]
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

        [DisplayName("Eastren Script Html For Songs, Order Now! google For Book")]
        public string? ScriptE { get; set; }

        [DisplayName("Westrean Script Html For Songs, Order Now! instagram For Book")]
        public string? ScriptW { get; set; }

        [DisplayName("Eastren Credits Html")]
        public string? CreditsE { get; set; }

        [DisplayName("Westrean Credits Html")]
        public string? CreditsW { get; set; }
        [DisplayName("Google Play Link For Software, Order Eastern Surit For Book")]

        public string? Link1 { get; set; }
        [DisplayName("App Store Link For Software, Order Western Surayt For Book")]
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
        [DisplayName("Image 1")]
        public string? img1 { get; set; }
        [DisplayName("Image 2")]
        public string? img2 { get; set; }
        [DisplayName("Image 3")]
        public string? img3 { get; set; }
        [DisplayName("Image 4")]
        public string? img4 { get; set; }
        [DisplayName("Image 5")]
        public string? img5 { get; set; }

        public string? Author { get; set; }
        [DisplayName("Designed By")]
        public string? DesignedBy { get; set; }
        [DisplayName("Sponsored By")]
        public string? SponsoredBY { get; set; }

        public string? Source { get; set; }
        [DisplayName("Features Html")]
        public string? Features { get; set; }
        [DisplayName("Product By")]
        public string? ProductBy { get; set; }
        [DisplayName("Created By")]
        public string? Created { get; set; }
        [DisplayName("Website Link For Software,Order From BetKanu shop For Book")]
        public string? Link3 { get; set; }
        public string? SongsList { get; set; }
        [DisplayName("Info 1 Html")]
        public string? Info1 { get; set; }
        [DisplayName("Info 2 Html")]
        public string? Info2 { get; set; }
        [DisplayName("Info 3 Html")]
        public string? Info3 { get; set; }

        [DisplayName("Is Release")]
        public bool IsRelease { get; set; }

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
