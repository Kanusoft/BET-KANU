﻿using BetKanu.Models.Common;

namespace BetKanu.Models.Interface
{
    public interface IProduct
    {
        List<Product>? GetAll();
        List<Product>? GetAll(Category category);
        Product? GetOne(int id);
        List<Product> RecentProduct(int num, Category category);
        ProductEpisode? GetOneEpisode(int? id);
        List<ProductEpisode> GetEpisode();
        List<ProductEpisode> GetallByParentId(int id);
        Product? GetoneProductByName(string? name);
        List<Product>? GetProductsByName(string? name);
        List<ProductEpisode> GetByParentIdandLang(int id, Language Lang);
    }
}
