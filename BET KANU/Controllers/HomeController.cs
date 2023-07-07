using BET_KANU.Models;
using BetKanu.Models.Interface;
using BetKanu.Models.ViewModels;
using BetKanu.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BET_KANU.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unit;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unit)
        {
            _logger = logger;
            _unit = unit;
        }

        public ActionResult Index()
        {
            var vm = new HomeViewModel();
            vm.Song = _unit.product.RecentProduct(1, Category.Songs);
            vm.Book = _unit.product.RecentProduct(1, Category.Books);
            vm.Software = _unit.product.RecentProduct(2, Category.Software);
            
            return View(vm);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult SocialMedia()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Support()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Reader()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Shop()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}