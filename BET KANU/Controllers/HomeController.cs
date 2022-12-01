using BET_KANU.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BET_KANU.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
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