using BetKanu.Models;
using BetKanu.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace BetKanu.Data.Repositories.Collaboration;

internal sealed class CollaborationContentInfo
{
    public CollaborationContentType ContentType { get; init; }
    public int ContentId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Group { get; init; } = string.Empty;
    public string? Subtitle { get; init; }
    public string? ImageUrl { get; init; }
    public string? Url { get; init; }
}

internal static class CollaborationContentLookup
{
    public static async Task<(
        IReadOnlyDictionary<int, Book> Books,
        IReadOnlyDictionary<int, Product> Products)> LoadAsync(
        BKdbContext db,
        IEnumerable<CollaborationProject> projects,
        CancellationToken ct)
    {
        var list = projects?.ToList() ?? new List<CollaborationProject>();

        var bookIds = list
            .Where(p => p.ContentType == CollaborationContentType.ReaderBook && p.ContentId.HasValue)
            .Select(p => p.ContentId!.Value)
            .Distinct()
            .ToList();

        var productIds = list
            .Where(p => p.ContentType == CollaborationContentType.Product && p.ContentId.HasValue)
            .Select(p => p.ContentId!.Value)
            .Distinct()
            .ToList();

        IReadOnlyDictionary<int, Book> books = bookIds.Count == 0
            ? new Dictionary<int, Book>()
            : await db.Books.AsNoTracking()
                .Where(b => bookIds.Contains(b.Id))
                .ToDictionaryAsync(b => b.Id, ct);

        IReadOnlyDictionary<int, Product> products = productIds.Count == 0
            ? new Dictionary<int, Product>()
            : await db.Products.AsNoTracking()
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, ct);

        return (books, products);
    }

    public static CollaborationContentInfo? Resolve(
        CollaborationContentType? contentType,
        int? contentId,
        IReadOnlyDictionary<int, Book>? books,
        IReadOnlyDictionary<int, Product>? products)
    {
        if (!contentType.HasValue || !contentId.HasValue || contentId.Value <= 0)
            return null;

        if (contentType == CollaborationContentType.ReaderBook)
        {
            Book? book = null;
            books?.TryGetValue(contentId.Value, out book);
            var title = book is not null && !string.IsNullOrWhiteSpace(book.Name)
                ? book.Name!
                : $"Book #{contentId.Value}";

            return new CollaborationContentInfo
            {
                ContentType = CollaborationContentType.ReaderBook,
                ContentId = contentId.Value,
                Title = title,
                Group = CollaborationContentSelection.GroupLabel(CollaborationContentType.ReaderBook),
                Subtitle = book?.Dialect,
                ImageUrl = book?.BookImageURL,
                Url = string.IsNullOrWhiteSpace(book?.ProjectURL) || book.ProjectURL == "#"
                    ? null
                    : book.ProjectURL
            };
        }

        if (contentType == CollaborationContentType.Product)
        {
            Product? product = null;
            products?.TryGetValue(contentId.Value, out product);
            var title = product is not null && !string.IsNullOrWhiteSpace(product.Title)
                ? product.Title!
                : $"Product #{contentId.Value}";

            return new CollaborationContentInfo
            {
                ContentType = CollaborationContentType.Product,
                ContentId = contentId.Value,
                Title = title,
                Group = CollaborationContentSelection.GroupLabel(
                    CollaborationContentType.Product,
                    product?.Category),
                Subtitle = product?.Category.ToString(),
                ImageUrl = !string.IsNullOrWhiteSpace(product?.CoverImage)
                    ? product!.CoverImage
                    : product?.SmallImage,
                Url = product is null ? null : $"/Products/details/{product.Id}"
            };
        }

        return null;
    }
}
