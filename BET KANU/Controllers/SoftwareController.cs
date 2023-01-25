using BET_KANU.ViewModels;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using BetKanu.Models.Common;

namespace BET_KANU.Controllers
{
    public class SoftwareController : Controller
    {
        private readonly IUnitOfWork _unit;
        public SoftwareController(IUnitOfWork unit)
        {
            _unit= unit;
        }
        public ActionResult Index()
        {
            var pvm = new ProductVM();
            pvm.products = _unit.product.GetAll(Category.Software);
            return View(pvm);
        }
    }
}
