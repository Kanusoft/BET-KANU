
namespace BetKanu.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Product>? Song { get; set; }
        public List<Product>? Book { get; set; }
        public List<Product>? Software { get; set; }

        public MagazineArticle? RecentArticle { get; set; }
    }
}
