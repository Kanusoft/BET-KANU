using BetKanu.Models;

namespace BET_KANU.ViewModels
{
    public class ProductVM
    {
        public Product? product { get; set; }
        public List<Product>? products { get; set; }

        public ProductEpisode? productEp { get; set; }
        public List<ProductEpisode>? episode { get; set; }
        public List<Shop>? Shops { get; set; }
        public Shop? Shop { get; set; }

    }
}
