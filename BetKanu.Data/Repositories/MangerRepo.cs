using BET_KANU.ViewModels;
using BetKanu.Models;
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
        public int Add(Product product)
        {
            if(product != null)
            {
                //product.Id = _bKdb.Products.Max(x => x.Id) + 1;
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

        public MangerVM GetProductInfo(int ProdId)
        {
            return new MangerVM() { product = GetOne(ProdId) };
        }
    }
}
