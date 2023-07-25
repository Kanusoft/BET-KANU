using BET_KANU.ViewModels;
using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using Microsoft.EntityFrameworkCore;

namespace BetKanu.Data.Repositories
{
    public class MangerRepo : IManger
    {
        private readonly BKdbContext _bKdb;

        public MangerRepo(BKdbContext bKdb)
        {
            _bKdb = bKdb;
        }

        public Product? GetOne(int id)
        {
            return _bKdb.Products.Where(p => p.Id == id).FirstOrDefault();
        }

        public List<Product> GetAll(string Select)
        {
            var product = from p in _bKdb.Products
                          select p;
            if (Select != null)
            {
                Category c = (Category)Enum.Parse(typeof(Category), Select);
                switch (Select)
                {
                    case "":
                        product = product.OrderBy(p => p.Title);
                        break;
                    case "Books":
                        product = product.Where(pe => pe.Category.Equals(c));
                        break;
                    case "Songs":
                        product = product.Where(pe => pe.Category.Equals(c));
                        break;
                    case "CartoonSeries":
                        product = product.Where(pe => pe.Category.Equals(c));
                        break;
                    case "Software":
                        product = product.Where(pe => pe.Category.Equals(c));
                        break;
                }
            }
            return product.ToList();
        }

        /// <summary>
        /// Products Management
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int Add(Product product)
        {
            if (product != null)
            {
                using (var dbTran = _bKdb.Database.BeginTransaction())
                {
                    try
                    {
                        _bKdb.Products.Add(product);
                        _bKdb.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (Exception)
                    {
                        dbTran.Rollback();
                    }
                }
                return product.Id;
            }
            return 0;
        }

        public bool Edit(Product product)
        {
            if (product != null)
            {
                try
                {
                    var existingProduct = _bKdb.Products.Find(product.Id);
                    if (existingProduct != null)
                    {
                        _bKdb.Entry(existingProduct).State = EntityState.Detached;
                        _bKdb.Products.Update(product);
                        _bKdb.SaveChanges();
                        return true;
                    }
                    else
                    {
                        _bKdb.Products.Update(product);
                        _bKdb.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public bool Delete(int id)
        {
            try
            {
                var prod = _bKdb.Products.FirstOrDefault(x => x.Id == id);
                if (prod != null)
                {
                    _bKdb.Remove(prod);
                    _bKdb.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool DoesExist(int id)
        {
            return _bKdb.Products.Any(p => p.Id == id);
        }

        /// <summary>
        /// Episodes Management
        /// </summary>
        /// <param name="episode"></param>
        /// <returns></returns>
        public int AddEpisode(ProductEpisode episode)
        {
            if (episode != null)
            {
                using (var dbTran = _bKdb.Database.BeginTransaction())
                {
                    try
                    {
                        _bKdb.Add(episode);
                        _bKdb.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (Exception)
                    {
                        dbTran.Rollback();
                    }
                }
                return episode.Id;
            }
            return 0;
        }

        public MangerVM GetProductInfo(int ProdId)
        {
            return new MangerVM() { product = GetOne(ProdId) };
        }

        public bool DeleteEP(int id)
        {
            try
            {
                var ep = _bKdb.ProductEpisodes.FirstOrDefault(e => e.Id == id);
                if (ep != null)
                {
                    _bKdb.Remove(ep);
                    _bKdb.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool EditEP(ProductEpisode episode)
        {
            if (episode != null)
            {
                try
                {
                    var exsitsepisode = _bKdb.ProductEpisodes.Find(episode.Id);
                    if (exsitsepisode != null) 
                    {
                        _bKdb.Entry(exsitsepisode).State = EntityState.Detached;
                        _bKdb.ProductEpisodes.Update(episode);
                    }
                    else
                    {
                        _bKdb.ProductEpisodes.Update(episode);
                    }
                    
                    _bKdb.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                }
            }
            return false;
        }

        /// <summary>
        /// Add items for sale
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public int Add(Shop shop)
        {
            if (shop != null)
            {
                using (var dbTran = _bKdb.Database.BeginTransaction())
                {
                    try
                    {
                        _bKdb.Add(shop);
                        _bKdb.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (Exception)
                    {
                        dbTran.Rollback();
                    }
                }
                return shop.Id;
            }
            return 0;
        }

        /// <summary>
        /// Edit the Sales
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool EditShop(Shop shop)
        {
            if (shop != null)
            {
                try
                {
                    _bKdb.Shops.Update(shop);
                    _bKdb.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        /// <summary>
        /// Delete the Sales
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteShop(int id)
        {
            try
            {
                var s = _bKdb.Shops.FirstOrDefault(e => e.Id == id);
                if (s != null)
                {
                    _bKdb.Remove(s);
                    _bKdb.SaveChanges();
                    return true;
                }

            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
