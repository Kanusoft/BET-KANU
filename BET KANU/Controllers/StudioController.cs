using BET_KANU.ViewModels;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using BetKanu.Models.Common;
namespace BET_KANU.Controllers
{
    public class StudioController : Controller
    {
        private readonly IUnitOfWork _unit;
        public StudioController(IUnitOfWork unit)
        {
            _unit= unit;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Songs()
        {
            ViewBag.Message = "Your application description page.";
            var prod = new ProductVM();
            prod.products = _unit.product.GetAll(Category.Songs);
            return View(prod);
        }

        public ActionResult CartoonSeries()
        {
            ViewBag.Message = "Your contact page.";
            var prod = new ProductVM();
            prod.products = _unit.product.GetAll(Category.CartoonSeries);
            return View(prod);
        }
        public ActionResult NinoandMia()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AlphabetSeries()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AlphabetSeriesDetails()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
