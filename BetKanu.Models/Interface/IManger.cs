using BET_KANU.ViewModels;


namespace BetKanu.Models.Interface
{
    public interface IManger
    {
        int Add(Product product);
        bool Edit(Product product);
        bool Delete(int id);
        MangerVM GetProductInfo(int ProdId);
        List<Product> GetAll(string Select);
        int AddEpisode(ProductEpisode episode);
        bool DeleteEP(int id);
        bool EditEP(ProductEpisode episode);

        int Add(Shop shop);
        bool EditShop(Shop shop);
        bool DeleteShop(int id);
    }
}
