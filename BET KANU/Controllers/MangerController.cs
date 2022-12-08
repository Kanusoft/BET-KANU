using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    public class MangerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MangerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            var Prod = _unitOfWork.product.GetAll();
            return View(Prod);
        }
        public ActionResult Create()
        {
            return View(new Product());
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.manger.Add(product);
            }
            return View(product);
        }
        public ActionResult Edit(int id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var prod = _unitOfWork.product.GetOne(id);
            if(prod == null)
            {
                return NotFound();
            }
            return View(prod);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.manger.Edit(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
