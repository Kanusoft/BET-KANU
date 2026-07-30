using BetKanu.Models.Common;

namespace BetKanu.Models.ViewModels.MagazineArticles
{
    public class MagazineArticlePreviewViewModel
    {
        public int Id { get; set; }

        public int MagazineId { get; set; }

        public string MagazineTitle { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string? Slug { get; set; }

        public string? ArticleNumber { get; set; }

        public string? EditionTitle { get; set; }

        public string? ArticleType { get; set; }

        public string? Summary { get; set; }

        public string? BannerImage { get; set; }

        public string? MobileBannerImage { get; set; }

        public string? EnglishTitle { get; set; }

        public string? EnglishIntroduction { get; set; }

        public string? EnglishBodyHtml { get; set; }

        public string? EnglishCreditsHtml { get; set; }

        public string? EnglishPdfUrl { get; set; }

        public string? WesternBodyHtml { get; set; }
        public string? WesternTitle { get; set; }

        public string? WesternIntroduction { get; set; }

        public string? EasternBodyHtml { get; set; }
        public string? EasternTitle { get; set; }

        public string? EasternIntroduction { get; set; }

        public string? WesternCreditsHtml { get; set; }

        public string? EasternCreditsHtml { get; set; }

        public string? WesternPdfUrl { get; set; }

        public string? EasternPdfUrl { get; set; }

        public MagazineArticleStatus Status { get; set; }
    }
}
