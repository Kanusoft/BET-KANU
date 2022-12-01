using BetKanu.Models;
using BetKanu.Models.Interface;
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
        public int Add(Product product)
        {
            if(product != null)
            {
                _bKdb.Products.Add(product);
                _bKdb.SaveChanges();
                return product.Id;
            }
            return 0;
        }
        public bool Edit(Product product)
        {
            try
            {
                _bKdb.Products.Update(product);
                _bKdb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

       
    }
}
