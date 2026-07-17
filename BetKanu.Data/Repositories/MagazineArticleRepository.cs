

using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BetKanu.Data.Repositories
{
    public class MagazineArticleRepository
        : IMagazineArticleRepository
    {
        private readonly BKdbContext _context;

        public MagazineArticleRepository(BKdbContext context)
        {
            _context = context;
        }

        public IEnumerable<MagazineArticle> GetAll()
        {
            return _context.MagazineArticles
                .AsNoTracking()
                .Include(a => a.Magazine)
                .OrderByDescending(a => a.ReleaseDate)
                .ThenBy(a => a.DisplayOrder)
                .ThenBy(a => a.ArticleNumber)
                .ToList();
        }

        public IEnumerable<MagazineArticle> GetByMagazineId(
            int magazineId)
        {
            return _context.MagazineArticles
                .AsNoTracking()
                .Where(a => a.MagazineId == magazineId)
                .OrderBy(a => a.DisplayOrder)
                .ThenBy(a => a.ArticleNumber)
                .ThenBy(a => a.ReleaseDate)
                .ToList();
        }

        public MagazineArticle? GetById(int id)
        {
            return _context.MagazineArticles
                .FirstOrDefault(a => a.Id == id);
        }

        public MagazineArticle? GetByIdWithMagazine(int id)
        {
            return _context.MagazineArticles
                .Include(a => a.Magazine)
                .FirstOrDefault(a => a.Id == id);
        }

        public MagazineArticle? GetBySlug(
            string magazineSlug,
            string articleSlug)
        {
            if (string.IsNullOrWhiteSpace(magazineSlug) ||
                string.IsNullOrWhiteSpace(articleSlug))
            {
                return null;
            }

            return _context.MagazineArticles
                .AsNoTracking()
                .Include(a => a.Magazine)
                .FirstOrDefault(a =>
                    a.Magazine.Slug == magazineSlug &&
                    a.Slug == articleSlug);
        }

        public MagazineArticle? GetPublishedBySlug(
            string magazineSlug,
            string articleSlug)
        {
            var nowUtc = DateTime.UtcNow;

            return _context.MagazineArticles
                .AsNoTracking()
                .Include(a => a.Magazine)
                .FirstOrDefault(a =>
                    a.Magazine.IsPublished &&
                    a.Magazine.Slug == magazineSlug &&
                    a.Slug == articleSlug &&
                    (
                        a.Status == MagazineArticleStatus.Published ||
                        (
                            a.Status == MagazineArticleStatus.Scheduled &&
                            a.PublishAtUtc.HasValue &&
                            a.PublishAtUtc.Value <= nowUtc
                        )
                    ));
        }

        public bool SlugExists(
            int magazineId,
            string slug,
            int? excludedArticleId = null)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return false;
            }

            return _context.MagazineArticles.Any(a =>
                a.MagazineId == magazineId &&
                a.Slug == slug &&
                (
                    !excludedArticleId.HasValue ||
                    a.Id != excludedArticleId.Value
                ));
        }

        public bool ArticleNumberExists(
            int magazineId,
            int articleNumber,
            int? excludedArticleId = null)
        {
            return _context.MagazineArticles.Any(a =>
                a.MagazineId == magazineId &&
                a.ArticleNumber == articleNumber &&
                (
                    !excludedArticleId.HasValue ||
                    a.Id != excludedArticleId.Value
                ));
        }

        public void Add(MagazineArticle article)
        {
            ArgumentNullException.ThrowIfNull(article);

            article.Title = article.Title.Trim();

            article.Slug = GenerateUniqueSlug(
                article.MagazineId,
                article.Title);

            _context.MagazineArticles.Add(article);
        }

        public void Update(MagazineArticle article)
        {
            ArgumentNullException.ThrowIfNull(article);

            article.Title = article.Title.Trim();

            article.Slug = GenerateUniqueSlug(
                article.MagazineId,
                article.Title,
                article.Id);

            _context.MagazineArticles.Update(article);
        }

        public void Remove(MagazineArticle article)
        {
            ArgumentNullException.ThrowIfNull(article);

            _context.MagazineArticles.Remove(article);
        }

        private string GenerateUniqueSlug(
            int magazineId,
            string title,
            int? excludedArticleId = null)
        {
            var baseSlug = GenerateSlug(title);

            if (string.IsNullOrWhiteSpace(baseSlug))
            {
                baseSlug = "article";
            }

            var slug = baseSlug;
            var suffix = 2;

            while (SlugExists(
                magazineId,
                slug,
                excludedArticleId))
            {
                slug = $"{baseSlug}-{suffix}";
                suffix++;
            }

            return slug;
        }

        private static string GenerateSlug(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalizedValue = value
                .Trim()
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormD);

            var builder = new StringBuilder();

            foreach (var character in normalizedValue)
            {
                var category =
                    CharUnicodeInfo.GetUnicodeCategory(character);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            var slug = builder
                .ToString()
                .Normalize(NormalizationForm.FormC);

            slug = Regex.Replace(
                slug,
                @"[^a-z0-9\s-]",
                string.Empty);

            slug = Regex.Replace(
                slug,
                @"\s+",
                "-");

            slug = Regex.Replace(
                slug,
                @"-+",
                "-");

            return slug.Trim('-');
        }

        public IEnumerable<MagazineArticle> GetPublishedByMagazineSlug(
    string magazineSlug)
        {
            if (string.IsNullOrWhiteSpace(magazineSlug))
            {
                return Enumerable.Empty<MagazineArticle>();
            }

            var nowUtc = DateTime.UtcNow;

            return _context.MagazineArticles
                .AsNoTracking()
                .Include(a => a.Magazine)
                .Where(a =>
                    a.Magazine.IsPublished &&
                    a.Magazine.Slug == magazineSlug &&
                    (
                        a.Status == MagazineArticleStatus.Published ||
                        (
                            a.Status == MagazineArticleStatus.Scheduled &&
                            a.PublishAtUtc.HasValue &&
                            a.PublishAtUtc.Value <= nowUtc
                        )
                    ))
                .OrderBy(a => a.DisplayOrder)
                .ThenBy(a => a.ArticleNumber)
                .ThenBy(a => a.ReleaseDate)
                .ToList();
        }
    }
}
