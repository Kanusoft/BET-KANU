using BetKanu.Models.Common;

namespace BetKanu.Models.ViewModels.MagazineArticles
{
    public class MagazineArticleListItemViewModel
    {
        public int Id { get; set; }

        public int MagazineId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? BannerImage { get; set; }

        public string? BannerImage350 { get; set; }

        public int ArticleNumber { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime ReleaseDate { get; set; }

        public MagazineArticleStatus Status { get; set; }

        public DateTime? PublishAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime? UpdatedAtUtc { get; set; }
    }
}
