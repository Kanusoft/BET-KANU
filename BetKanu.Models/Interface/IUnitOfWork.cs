
namespace BetKanu.Models.Interface
{
    public interface IUnitOfWork
    {
        public IBKBundle bKBundle { get;}
        public IProduct product { get;}
        public IManger manger { get;}
        public IShop Shop { get;}
        IMagazineRepository Magazines { get; }
        IMagazineArticleRepository MagazineArticles { get; }
    }
}
