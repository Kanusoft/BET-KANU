using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    public class StudioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Songs()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult CartoonSeries()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
