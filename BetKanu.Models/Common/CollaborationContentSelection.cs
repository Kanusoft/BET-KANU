namespace BetKanu.Models.Common
{
    public static class CollaborationContentSelection
    {
        public const string ProductPrefix = "product";
        public const string ReaderBookPrefix = "readerbook";

        public static string Format(CollaborationContentType type, int contentId) =>
            type switch
            {
                CollaborationContentType.Product => $"{ProductPrefix}:{contentId}",
                CollaborationContentType.ReaderBook => $"{ReaderBookPrefix}:{contentId}",
                _ => string.Empty
            };

        public static string? Format(CollaborationContentType? type, int? contentId)
        {
            if (!type.HasValue && !contentId.HasValue)
                return null;

            if (!type.HasValue || !contentId.HasValue || contentId.Value <= 0)
                return null;

            return Format(type.Value, contentId.Value);
        }

        public static bool TryParse(
            string? selection,
            out CollaborationContentType? contentType,
            out int? contentId,
            out string? error)
        {
            contentType = null;
            contentId = null;
            error = null;

            if (string.IsNullOrWhiteSpace(selection))
                return true;

            var parts = selection.Trim().Split(':', 2, StringSplitOptions.TrimEntries);
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
            {
                error = "The selected content is invalid.";
                return false;
            }

            if (!int.TryParse(parts[1], out var id) || id <= 0)
            {
                error = "The selected content is invalid.";
                return false;
            }

            switch (parts[0].ToLowerInvariant())
            {
                case ProductPrefix:
                    contentType = CollaborationContentType.Product;
                    contentId = id;
                    return true;
                case ReaderBookPrefix:
                    contentType = CollaborationContentType.ReaderBook;
                    contentId = id;
                    return true;
                default:
                    error = "The selected content type is not supported.";
                    return false;
            }
        }

        public static string GroupLabel(CollaborationContentType type, Category? productCategory = null)
        {
            if (type == CollaborationContentType.ReaderBook)
                return "BK Reader Books";

            return productCategory switch
            {
                Category.Books => "Products — Books",
                Category.Songs => "Products — Songs",
                Category.CartoonSeries => "Products — Cartoon Series",
                Category.Software => "Products — Software",
                Category.Collection => "Products — Collections",
                _ => "Products"
            };
        }
    }
}
