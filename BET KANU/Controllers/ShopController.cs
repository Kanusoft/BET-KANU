using BET_KANU.ViewModels;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unit;
        public ShopController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public ActionResult Index()
        {
            //var svm = new ProductVM();
            //svm.Shops = _unit.Shop.GetAll();
            return Redirect("https://betkanu.square.site/s/shop");
          //  return View(svm);
        }
    }
}
