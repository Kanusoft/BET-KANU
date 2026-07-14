using BetKanu.Data.Repositories;
using BetKanu.Models.Interface;


namespace BetKanu.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BKdbContext _dbContext;
        public IBKBundle bKBundle { get; }
        public IProduct product { get; }
        public IManger manger { get; }
        public IShop Shop { get; }
        public IMagazineRepository Magazines { get; }

        public IMagazineArticleRepository MagazineArticles
        {
            get;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public UnitOfWork(BKdbContext dbContext)
        {
            _dbContext = dbContext;
            bKBundle = new BookBundleDAO(dbContext);
            product = new ProductDAO(dbContext);
            manger = new MangerRepo(dbContext);
            Shop= new ShopRepo(dbContext);

            Magazines = new MagazineRepository(dbContext);

            MagazineArticles =
                new MagazineArticleRepository(dbContext);
        }
    }
}
