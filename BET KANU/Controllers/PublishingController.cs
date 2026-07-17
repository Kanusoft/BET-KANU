using BET_KANU.ViewModels;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    public class PublishingController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _env;

        public PublishingController(IUnitOfWork unit, IWebHostEnvironment env)
        {
            _unit = unit;
            _env = env;
        }

        public IActionResult Book()
        {
            var pvm = new ProductVM();
            pvm.products = _unit.product.GetAll(Category.Books);
            return View(pvm);
        }

        [HttpGet]
        public ActionResult Articles(string slug)
        {
            var MagazineArticles = _unit.MagazineArticles.GetPublishedByMagazineSlug(slug);
           return View(MagazineArticles); 
        }

        [HttpGet("magazine/{magazineSlug}/{articleSlug}")]
        public ActionResult ArticleDetail(string magazineSlug, string articleSlug)
        {
            if (string.IsNullOrWhiteSpace(magazineSlug) ||
                string.IsNullOrWhiteSpace(articleSlug))
            {
                return RedirectToAction("Articles");
            }

            var article = _unit.MagazineArticles.GetPublishedBySlug(
                magazineSlug,
                articleSlug);

            if (article == null)
            {
                return RedirectToAction("Articles");
            }

            return View(article);
        }

        public ActionResult Magazine() 
        {
            var magazines = _unit.Magazines.GetAll();

            return View(magazines);
        }

        [HttpGet]
        public IActionResult DownloadArticlePdf(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                return BadRequest();

            file = Path.GetFileName(file); 

            var fullPath = Path.Combine(_env.WebRootPath, "Articles-pdf", file);

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            return PhysicalFile(fullPath, "application/pdf", file);
        }
    }
}
