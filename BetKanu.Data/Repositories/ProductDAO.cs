﻿using BetKanu.Models;
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

        public IEnumerable<Product> GetAll()
        {
            return _db.Products.ToList();
        }
        public List<Product> GetAll(Category category , Target target)
        {
            return _db.Products.Where(p => p.Category == category && p.TargetAudince == target).ToList();
        }
        public Product GetOne(int id)
        {
            return _db.Products.Where(p => p.Id == id).FirstOrDefault();
        }

        public List<Product> RecentProduct()
        {
            return _db.Products.OrderByDescending(p => p.ReleaseDate).ToList();
        }

        public List<ProductEpisode> GetEpisode(int id)
        {
            return _db.ProductEpisodes.Where(p => p.ProductId == id).ToList();
        }
    }
}
