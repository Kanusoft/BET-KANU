using BET_KANU.ViewModels;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    public class PublishingController : Controller
    {
        private readonly IUnitOfWork _unit;
        public PublishingController(IUnitOfWork unit)
        {
            _unit= unit;
        }
        public IActionResult Index()
        {
            var pvm = new ProductVM();
            pvm.products = _unit.product.GetAll(Category.Books);
            return View(pvm);
        }
    }
}
