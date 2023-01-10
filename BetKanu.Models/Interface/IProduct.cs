using BetKanu.Models.Common;

namespace BetKanu.Models.Interface
{
    public interface IProduct
    {
        List<Product>? GetAll();
        List<Product>? GetAll(Category category, Target target);
        Product? GetOne(int id);
        List<Product>? RecentProduct(int num);
    }
}
