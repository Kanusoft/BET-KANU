using BetKanu.Models;
using BetKanu.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Data.Repositories
{
    public class ShopRepo : IShop
    {
        private readonly BKdbContext _db;
        public ShopRepo(BKdbContext db)
        {
            _db = db;
        }

        public List<Shop> GetAll()
        {
            return _db.Shops.ToList();
        }

        public Shop Getone(int id)
        {
            return _db.Shops.FirstOrDefault(s => s.Id == id);
        }
    }
}
