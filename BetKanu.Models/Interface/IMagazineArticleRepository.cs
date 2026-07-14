

namespace BetKanu.Models.Interface
{
    public interface IMagazineArticleRepository
    {
        IEnumerable<MagazineArticle> GetAll();

        IEnumerable<MagazineArticle> GetByMagazineId(
            int magazineId);

        MagazineArticle? GetById(int id);

        MagazineArticle? GetByIdWithMagazine(int id);

        MagazineArticle? GetBySlug(
            string magazineSlug,
            string articleSlug);

        bool SlugExists(
            int magazineId,
            string slug,
            int? excludedArticleId = null);

        bool ArticleNumberExists(
            int magazineId,
            int articleNumber,
            int? excludedArticleId = null);

        void Add(MagazineArticle article);

        void Update(MagazineArticle article);

        void Remove(MagazineArticle article);

        MagazineArticle? GetPublishedArticle(
    string magazineSlug,
    string articleSlug);
    }
}
