using BET_KANU.Services;
using BetKanu.Models.Common;
using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BET_KANU.ViewModels;

namespace BET_KANU.Controllers
{
    [Authorize]
    public class CartoonController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IBlobStorageService _blob;
        public CartoonController(IUnitOfWork unit, IBlobStorageService blob)
        {
            _unit = unit;
            _blob = blob;
        }

        [HttpGet]
        public ActionResult CreateCartoonSeries()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCartoonSeries([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,VideoE,VideoW,ViewsE,ViewsW")]Product cartoonseries)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (cartoonseries.SmallUrl != null)
                    {
                        string ImagePath = await SaveImage(cartoonseries.SmallUrl);
                        cartoonseries.SmallImage = ImagePath;
                    }
                    if (cartoonseries.CoverUrl != null)
                    {
                        string ImagePath = await SaveImage(cartoonseries.CoverUrl);
                        cartoonseries.CoverImage = ImagePath;
                    }

                    cartoonseries.Category = Category.CartoonSeries;
                    var result = _unit.manger.Add(cartoonseries);
                    if (result > 0)
                    {
                        TempData["Messagee"] = cartoonseries.Title + " " + "has been created successfully!";
                        return RedirectToAction("EditCartoonSeries", new { id = cartoonseries.Id });
                    }
                }
                catch (Exception)
                {
                    TempData["AlertMessage"] = "Ooops something went wrong !!";
                    return View(cartoonseries);
                }
            }
            return View(cartoonseries);
        }

        [HttpGet]
        public ActionResult EditCartoonSeries(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var vm = new MangerVM();
            vm.product = _unit.product.GetOne(id);
            vm.productEpisode = _unit.product.GetallByParentId(id);
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCartoonSeries(MangerVM prod)
        {
            if (ModelState.IsValid && prod.product != null)
            {
                try
                {
                    if (prod.product.SmallUrl != null)
                    {
                        string oldfile = _unit.product.GetOne(prod.product.Id)?.SmallImage ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blob.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.product.SmallUrl);
                        prod.product.SmallImage = Imagepath;
                    }

                    if (prod.product.CoverUrl != null)
                    {
                        string oldfile = _unit.product.GetOne(prod.product.Id)?.CoverImage ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blob.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.product.CoverUrl);
                        prod.product.CoverImage = Imagepath;
                    }


                    if (_unit.manger.Edit(prod.product))
                    {
                        return RedirectToAction("Index", "Manger");
                    }
                }
                catch (Exception)
                {
                    TempData["AlertMessage"] = "Ooops something went wrong !!";
                    return View(prod);
                }
            }
            return View(prod);
        }

        [HttpGet]
        public ActionResult CreateEpisode(int parentId)
        {
            var parentProduct = _unit.product.GetOne(parentId);
            ViewBag.parentname = parentProduct?.Title?.ToString();
            ViewBag.ProductId = parentId;  
            return View(new ProductEpisode());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEpisode([Bind("Title,Views,VideoE,VideoW,ImageE,ImageW,Status,ReleaseDate,ProductId,EastrenImageFile,WestreanImageFile")]ProductEpisode cartoonepisode, int parentId)
        {
            if (ModelState.IsValid)
            {

                if (cartoonepisode.EastrenImageFile != null)
                {
                    string ImagePath = await SaveImage(cartoonepisode.EastrenImageFile);
                    cartoonepisode.ImageE = ImagePath;
                }

                if (cartoonepisode.WestreanImageFile != null)
                {
                    string ImagePath = await SaveImage(cartoonepisode.WestreanImageFile);
                    cartoonepisode.ImageW = ImagePath;
                }

                var result = _unit.manger.AddEpisode(cartoonepisode);
                if (result > 0)
                {
                    TempData["Messagee"] = cartoonepisode.Title + " " + "has been created successfully!";
                    return RedirectToAction("Index", "Manger");
                }
                else
                {
                    var parentProduct = _unit.product.GetOne(parentId);
                    ViewBag.parentId = parentProduct?.Title?.ToString();
                    ViewBag.ProductId = parentId;
                    return View(cartoonepisode);
                }
            }
            else
            {
                var parentProduct = _unit.product.GetOne(parentId);
                ViewBag.parentId = parentProduct?.Title?.ToString();
                ViewBag.ProductId = parentId;
                return View(cartoonepisode);
            }
        }



        [HttpGet]
        public ActionResult EditEpisode(int id, int parentId)
        {
            var parentProduct = _unit.product.GetOne(parentId);
            ViewBag.parentname = parentProduct?.Title?.ToString();
            ViewBag.parentId = parentId;
            if (id == 0)
            {
                return NotFound();
            }
            var prodEp = _unit.product.GetOneEpisode(id);
            if (prodEp == null)
            {
                return NotFound();
            }
            return View(prodEp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEpisode([Bind("Id,Title,Views,VideoE,VideoW,ImageE,ImageW,Status,ReleaseDate,ProductId,EastrenImageFile,WestreanImageFile")]ProductEpisode episode, int parentId)
        {
            if (ModelState.IsValid)
            {
                if (episode.EastrenImageFile != null)
                {
                    string oldfile = _unit.product.GetOneEpisode(episode.Id)?.ImageE ?? string.Empty;
                    if (!string.IsNullOrEmpty(oldfile))
                    {
                        await _blob.DeleteDocumentAsync(oldfile);
                    }
                    var Imagepath = await SaveImage(episode.EastrenImageFile);
                    episode.ImageE = Imagepath;
                }
                else
                {
                    var currentImage = _unit.product?.GetOneEpisode(episode.Id)?.ImageE ?? string.Empty;
                    episode.ImageE = currentImage;
                }
                if (episode.WestreanImageFile != null)
                {
                    string oldfile = _unit?.product?.GetOneEpisode(episode.Id)?.ImageW ?? string.Empty;
                    if (!string.IsNullOrEmpty(oldfile))
                    {
                        await _blob.DeleteDocumentAsync(oldfile);
                    }
                    var Imagepath = await SaveImage(episode.WestreanImageFile);
                    episode.ImageW = Imagepath;
                }
                else
                {
                    var currentImage = _unit.product?.GetOneEpisode(episode.Id)?.ImageW ?? string.Empty;
                    episode.ImageW = currentImage;
                }

                if (_unit.manger.EditEP(episode))
                {
                    return RedirectToAction("Index", "Manger");
                }
                else
                {
                    var parentProduct = _unit.product.GetOne(parentId);
                    ViewBag.parentname = parentProduct?.Title?.ToString();
                    ViewBag.parentId = parentId;
                    return View(episode);
                }
            }
            else
            {
                var parentProduct = _unit.product.GetOne(parentId);
                ViewBag.parentname = parentProduct?.Title?.ToString();
                ViewBag.parentId = parentId;
                return View(episode);
            }
        }

        public ActionResult DeleteEpisode(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var prodEp = _unit.product.GetOneEpisode(id);
            if (prodEp == null)
            {
                return NotFound();
            }
            return View(prodEp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteEPpost(int id, int parentId)
        {
            if (ModelState.IsValid)
            {
                var westreanimg = _unit.product.GetOneEpisode(id)?.ImageW ?? string.Empty;
                if (westreanimg != null && westreanimg != "")
                {
                    await _blob.DeleteDocumentAsync(westreanimg);
                }

                var eastrenimg = _unit.product.GetOneEpisode(id)?.ImageE ?? string.Empty;
                if (eastrenimg != null && eastrenimg != "")
                {
                    await _blob.DeleteDocumentAsync(eastrenimg);
                }

                if (_unit.manger.DeleteEP(id))
                {
                    TempData["Message"] = "The Product has been Delete successfully!";
                }
            }
            return RedirectToAction("EditCartoonSeries", new { id = parentId });
        }

        /// <summary>
        /// Save the Images to the Blob storage at products Container
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private async Task<string> SaveImage(IFormFile image)
        {
            string Pathimg = @"https://betkanublob.blob.core.windows.net/betkanublob/products/";
            string filename = Path.GetFileNameWithoutExtension(image.FileName);
            string ex = Path.GetExtension(image.FileName);
            string ImgName = Pathimg + filename + ex;
            await _blob.UploadBlobImageAsync(image);
            return ImgName;
        }
    }
}
