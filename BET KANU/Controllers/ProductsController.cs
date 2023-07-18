﻿using BET_KANU.ViewModels;
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

      
        public ActionResult Index1()
        {
            var p = new ProductVM();
            p.products = _unit.product.GetAll();
            return View(p);
        }

        [HttpGet]
        public ActionResult details(int id)
        {
            var vm = new ProductVM();
            vm.product = _unit.product.GetOne(id);
            vm.episode = _unit.product.GetallByParentId(id);
            return View(vm);
        }

    }
}
