

using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using Microsoft.EntityFrameworkCore;

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
                .ThenBy(a => a.Title)
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

            _context.MagazineArticles.Add(article);
        }

        public void Update(MagazineArticle article)
        {
            ArgumentNullException.ThrowIfNull(article);

            _context.MagazineArticles.Update(article);
        }

        public void Remove(MagazineArticle article)
        {
            ArgumentNullException.ThrowIfNull(article);

            _context.MagazineArticles.Remove(article);
        }

        public MagazineArticle? GetPublishedArticle(
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
    }
}
