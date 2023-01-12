using BET_KANU.ViewModels;
using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Data.Repositories
{
    public class MangerRepo : IManger
    {
        private readonly BKdbContext _bKdb;

        public MangerRepo(BKdbContext bKdb)
        {
          _bKdb = bKdb;
        }

        public Product GetOne(int id)
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
        public int Add(Product product)
        {
            if(product != null)
            {               
                _bKdb.Products.Add(product);
                _bKdb.SaveChanges();  
            }            
            return 0;
        }
        public bool Edit(Product product)
        {
            try
            {

                _bKdb.Products.Update(product);
                _bKdb.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(int id)
        {
            try
            {
                var prod = _bKdb.Products.FirstOrDefault(x => x.Id == id);
                _bKdb.Remove(prod);
                _bKdb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        public bool DoesExist(int id)
        {
            return _bKdb.Products.Any(p => p.Id == id);
        }
        
        public bool Edit(MangerVM manger)
        {
            if(manger != null && DoesExist(manger.product.Id))
            {
                manger.product.ReleaseDate = (manger.product.ReleaseDate == null || manger.product.ReleaseDate == null ? DateTime.Now : manger.product.ReleaseDate);
                _bKdb.Entry(manger.product).State = EntityState.Modified;
                _bKdb.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }            
        }

        public int Add(ProductEpisode episode)
        {
            if(episode != null)
            {
                _bKdb.Add(episode);
                _bKdb.SaveChanges();
            }
            return 0;
        }

        public MangerVM GetProductInfo(int ProdId)
        {
            return new MangerVM() { product = GetOne(ProdId) };
        }

        public bool DeleteEP (int id)
        {
            try
            {
                var ep = _bKdb.ProductEpisodes.FirstOrDefault(e => e.Id == id);
                _bKdb.Remove(ep);
                _bKdb.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }            
        }

        public bool EditEP(ProductEpisode episode)
        {
            try
            {
                _bKdb.ProductEpisodes.Update(episode);
                _bKdb.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
