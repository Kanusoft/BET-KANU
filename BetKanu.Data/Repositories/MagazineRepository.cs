

using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using Microsoft.EntityFrameworkCore;

namespace BetKanu.Data.Repositories
{
    public class MagazineRepository : IMagazineRepository
    {

        private readonly BKdbContext _context;

        public MagazineRepository(BKdbContext context)
        {
            _context = context;
        }

        public IEnumerable<Magazine> GetAll()
        {
            return _context.Magazines
                .AsNoTracking()
                .Include(m => m.Articles)
                .OrderBy(m => m.DisplayOrder)
                .ThenByDescending(m => m.Year)
                .ThenBy(m => m.Title)
                .ToList();
        }

        public Magazine? GetById(int id)
        {
            return _context.Magazines
                .FirstOrDefault(m => m.Id == id);
        }

        public Magazine? GetBySlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return null;
            }

            return _context.Magazines
                .AsNoTracking()
                .FirstOrDefault(m => m.Slug == slug);
        }

        public Magazine? GetByIdWithArticles(int id)
        {
            return _context.Magazines
                .Include(m => m.Articles
                    .OrderBy(a => a.DisplayOrder)
                    .ThenBy(a => a.ArticleNumber))
                .FirstOrDefault(m => m.Id == id);
        }

        public bool SlugExists(
            string slug,
            int? excludedMagazineId = null)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return false;
            }

            return _context.Magazines.Any(m =>
                m.Slug == slug &&
                (
                    !excludedMagazineId.HasValue ||
                    m.Id != excludedMagazineId.Value
                ));
        }

        public void Add(Magazine magazine)
        {
            ArgumentNullException.ThrowIfNull(magazine);

            _context.Magazines.Add(magazine);
        }

        public void Update(Magazine magazine)
        {
            ArgumentNullException.ThrowIfNull(magazine);

            _context.Magazines.Update(magazine);
        }

        public void Remove(Magazine magazine)
        {
            ArgumentNullException.ThrowIfNull(magazine);

            _context.Magazines.Remove(magazine);
        }

        public IEnumerable<Magazine> GetPublished()
        {
            return _context.Magazines
                .AsNoTracking()
                .Where(m => m.IsPublished)
                .Include(m => m.Articles.Where(a =>
                    a.Status == MagazineArticleStatus.Published))
                .OrderBy(m => m.DisplayOrder)
                .ThenByDescending(m => m.Year)
                .ToList();
        }

        public Magazine? GetPublishedBySlug(string slug)
        {
            return _context.Magazines
                .AsNoTracking()
                .Where(m =>
                    m.IsPublished &&
                    m.Slug == slug)
                .Include(m => m.Articles.Where(a =>
                    a.Status == MagazineArticleStatus.Published))
                .FirstOrDefault();
        }
    }
}
