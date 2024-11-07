using BetKanu.Models.Common;
using Microsoft.AspNetCore.Http;
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

        [DisplayName("Eastern Script Html For Songs, Order Now! google For Book")]
        public string? ScriptE { get; set; }

        [DisplayName("Western Script Html For Songs, Order Now! instagram For Book")]
        public string? ScriptW { get; set; }

        [DisplayName("Eastern Credits Html")]
        public string? CreditsE { get; set; }

        [DisplayName("Western Credits Html")]
        public string? CreditsW { get; set; }
        [DisplayName("App Store  Link For Software, Order Eastern Surit For Book")]

        public string? Link1 { get; set; }
        [DisplayName("Google Play Link For Software, Order Western Surayt For Book")]
        public string? Link2 { get; set; }

        [DisplayName("Western Pdf Link")]
        public string? Wpdf { get; set; }

        [DisplayName("Eastern Pdf Link")]
        public string? Epdf { get; set; }

        [DisplayName("Eastern Video Link")]
        public string? VideoE { get; set; }

        [DisplayName("Western Video Link")]
        public string? VideoW { get; set; }

        [DisplayName("Eastern Video Views")]
        public int ViewsE { get; set; }

        [DisplayName("Western Video Views")]
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

        [DisplayName("Release")]
        public bool IsRelease { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? SmallUrl { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? CoverUrl { get; set; }

        [DisplayName("Malouli Script Html")]
        public string? MalouliScript { get; set; }

        [DisplayName("English Script")]
        public string? EnglishScript { get; set; }

        [DisplayName("Malouli Credits HTML")]
        public string? CreditsM { get; set; }
        [DisplayName("Malouli Video Link")]
        public string? VideoM { get; set; }
        [DisplayName("Malouli Video Views")]
        public int? ViewsM { get; set; }
        [DisplayName("Malouli Pdf Link")]
        public string? Mpdf { get; set; }

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
        [NotMapped]
        [DisplayName("Malouli Pdf File")]
        public IFormFile? MalouliPdfFile { get; set; }


        //public int? ParentId { get; set; }
        //public Product? Parent { get; set; }
    }
}
