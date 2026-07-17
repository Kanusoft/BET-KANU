

namespace BetKanu.Models.ViewModels
{
    public class MagazineListItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public int Year { get; set; }

        public int DisplayOrder { get; set; }

        public string? CoverImage { get; set; }

        public string? CoverImage350 { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public int ArticleCount { get; set; }
    }
}
