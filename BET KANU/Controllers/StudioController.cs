using BET_KANU.ViewModels;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using BetKanu.Models.Common;
using BetKanu.Models.ViewModels;

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
            var VM = new StudioViewModel();
            VM.Products = _unit.product.GetProductsByName("NINO & MIA");
            return View(VM);
        }


        public ActionResult AlphabetSeries()
        {
            ViewBag.Message = "Your contact page.";
            var VM = new StudioViewModel();
            VM.product = _unit.product.GetoneProductByName("Alphabet Series");
            if(VM.product != null)
            {
                int Id = VM.product.Id;
                VM.WestrenEpisodes = _unit.product.GetByParentIdandLang(Id,Language.Westren);
                VM.EastrenEpisodes = _unit.product.GetByParentIdandLang(Id, Language.Eastren);
            }
            
            return View(VM);
        }
      
    }
}
