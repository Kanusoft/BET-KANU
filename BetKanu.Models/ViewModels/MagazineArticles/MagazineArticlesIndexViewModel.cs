namespace BetKanu.Models.ViewModels.MagazineArticles
{
    public class MagazineArticlesIndexViewModel
    {
        public int MagazineId { get; set; }

        public string MagazineTitle { get; set; } = string.Empty;

        public string MagazineSlug { get; set; } = string.Empty;

        public bool MagazineIsPublished { get; set; }

        public List<MagazineArticleListItemViewModel> Articles { get; set; }
            = new();
    }
}
