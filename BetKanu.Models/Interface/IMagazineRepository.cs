
namespace BetKanu.Models.Interface
{
    public interface IMagazineRepository
    {
        IEnumerable<Magazine> GetAll();

        Magazine? GetById(int id);

        Magazine? GetBySlug(string slug);

        Magazine? GetByIdWithArticles(int id);

        bool SlugExists(
            string slug,
            int? excludedMagazineId = null);

        void Add(Magazine magazine);

        void Update(Magazine magazine);

        void Remove(Magazine magazine);
    }
}
