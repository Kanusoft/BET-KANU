using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;


namespace BetKanu.Data.Repositories
{
    public class ProductDAO : IProduct
    {
        private readonly BKdbContext _db;
        public ProductDAO(BKdbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get list of Newest products 
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            return _db.Products.OrderByDescending(p => p.ReleaseDate).ToList();
                     
        }

        /// <summary>
        /// Get list of Products by Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Product> GetAll(Category category)
        {
            return _db.Products.Where(p => p.Category == category).OrderByDescending(p => p.ReleaseDate).ToList();
        }

        /// <summary>
        /// Get one Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product? GetOne(int id)
        {
            return _db.Products.Where(p => p.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get the Last ( Number ) of new Products 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<Product> RecentProduct(int num, Category category)
        {
            return _db.Products.Where(P => P.Category == category).OrderByDescending(p => p.ReleaseDate).Take(num).ToList();
        }
        
        public Product? GetoneProductByName(string? name)
        {
            return _db.Products.Where(p => p.Title == name).FirstOrDefault();
        }
        public List<Product>? GetProductsByName(string? name)
        {
            return _db.Products
            .Where(p => p.Title.StartsWith(name))
            .ToList();
        }

        /// <summary>
        /// Get List of cartoon Episodes
        /// </summary>
        /// <returns></returns>
        public List<ProductEpisode> GetEpisode()
        {
            return _db.ProductEpisodes.ToList();
        }

        /// <summary>
        /// Get all Episodes by Product parent Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductEpisode>GetallByParentId(int id)
        {
            return _db.ProductEpisodes.Where(pe => pe.ProductId== id).ToList();
        }

        public List<ProductEpisode> GetByParentIdandLang(int id , Language Lang) 
        {
            return _db.ProductEpisodes.Where(pe => pe.ProductId == id && pe.Status == Lang).OrderBy(pe => pe.ReleaseDate).ToList();
        }
        /// <summary>
        /// Get one Episode by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductEpisode? GetOneEpisode(int? id)
        {
            return _db.ProductEpisodes.FirstOrDefault(e => e.Id == id);
        }


       
    }
}
