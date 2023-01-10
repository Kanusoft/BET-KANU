using BetKanu.Models;

namespace BET_KANU.ViewModels
{
    public class ProductVM
    {
        public Product? product { get; set; } 
        public List<Product>? products { get; set; }
        public ProductEpisode? episode { get; set; }
    }
}
