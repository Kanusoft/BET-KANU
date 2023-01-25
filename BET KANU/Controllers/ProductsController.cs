using BET_KANU.ViewModels;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unit;
        public ProductsController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index1()
        {
            var p = new ProductVM();
            p.products = _unit.product.GetAll();
            return View(p);
        }
        public ActionResult details(int id)
        {
            var vm = new ProductVM();
            vm.product = _unit.product.GetOne(id);
            vm.episode = _unit.product.GetallByParentId(id);
            //var p = _unit.product.GetOne(id);
            return View(vm);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Bethnahrin()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult BirdSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Birthdaysong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Colorssong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Fingerfamily()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Kanuguessapp()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Marysong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Mondayschildsong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Mshihosong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Numbersong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Radiokifoapp()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Santasong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Shapessong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Alphabetsong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult BodyPartssong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult Johnnyjohnnysong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult Rowyourboatsong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult Allalayesong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult FruitsSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult Ninandmiacoloringbook()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult BETKANUconversationbooklet()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult Deckthehalls()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult HassisanSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult EveryoneHasaCarSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult SeasonsSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }

        public ActionResult MyUncleSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult Washyourhandssong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult MomisComigNowsong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult BedtimePrayerSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult TodayIsNewYearsSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult AnimalSoundsSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult MothersSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult AntSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult JosephtheGardenerSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult CanopyofUnity()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult BETKANUReaderApp()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult BETKANUColoringBook()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult KidsSongsBook()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult PenandPaperBook()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult UrhoySchooCurriculum()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult openshutthemsong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult WeThreeKingsSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult TheSunSetSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult HolaHolaSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult FreezeDanceSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult AuClairdelaluneSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult AscensionSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult GrandmaSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult MamaSemlaMkhloSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult SoghithoSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult BETKANUNames()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult DeliciousFoodSong()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult TheNecklace()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult AuClairDeLaLune()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
        public ActionResult BETKANUSongsColoringBookAlbumVol1()
        {
            ViewBag.Message = "Product details page.";

            return View();
        }
    }
}
