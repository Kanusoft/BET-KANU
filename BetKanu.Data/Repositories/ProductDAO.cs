using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Data.Repositories
{
    public class ProductDAO : IProduct
    {
        private readonly BKdbContext _db;
        public ProductDAO(BKdbContext db)
        {
            _db = db;
        }

        public List<Product> GetAll()
        {
            return _db.Products.OrderByDescending(p => p.ReleaseDate).ToList();
                     
        }
        public List<Product> GetAll(Category category)
        {
            return _db.Products.Where(p => p.Category == category).ToList();
        }
        public Product GetOne(int id)
        {
            return _db.Products.Where(p => p.Id == id).FirstOrDefault();
        }
        public List<Product> RecentProduct(int num)
        {
            return _db.Products.OrderByDescending(p => p.ReleaseDate).Take(num).ToList();
        }

        public List<ProductEpisode> GetEpisode()
        {
            //.Where(p => p.ProductId == id)
            return _db.ProductEpisodes.ToList();
        }

        public List<ProductEpisode>GetallByParentId(int id)
        {
            return _db.ProductEpisodes.Where(pe => pe.ProductId== id).ToList();
        }

        public ProductEpisode GetOneEpisode(int id)
        {
            return _db.ProductEpisodes.FirstOrDefault((e => e.Id == id));
        }
    }
}
