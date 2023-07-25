
using BetKanu.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BET_KANU.ViewModels
{
    public class MangerVM
    {
        public Product? product { get; set; }

        public List<ProductEpisode>? productEpisode { get; set; }

        [NotMapped]
        public IFormFile? SmallUrl { get; set; }

        [NotMapped]
        public IFormFile? CoverUrl { get; set; }
    }
}
