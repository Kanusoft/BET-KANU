using BetKanu.Data.Repositories;
using BetKanu.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BKdbContext _dbContext;
        public IBKBundle bKBundle { get; }
        public IProduct product { get; }
        public IManger manger { get; }
        public IShop Shop { get; }

        public UnitOfWork(BKdbContext dbContext)
        {
            _dbContext = dbContext;
            bKBundle = new BookBundleDAO(dbContext);
            product = new ProductDAO(dbContext);
            manger = new MangerRepo(dbContext);
            Shop= new ShopRepo(dbContext);
        }
    }
}
