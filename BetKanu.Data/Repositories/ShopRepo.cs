using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;


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

        public List<Shop> RecentProduct(int num)
        {
            return _db.Shops.OrderByDescending(p => p.ReleaseDate).Take(num).ToList();
        }
    }
}
