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

        public ActionResult Article() => View();
        public ActionResult ArticleDetail() => View();
        public ActionResult ArticleDetail2() => View();
        public ActionResult ArticleDetail3() => View();
        public ActionResult ArticleDetail4() => View();
        public ActionResult ArticleDetail5() => View();
        public ActionResult ArticleDetail6() => View();
        public ActionResult Magazine() => View();

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
