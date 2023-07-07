using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BET_KANU.Services;
using BET_KANU.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using BetKanu.Models.Common;

namespace BET_KANU.Controllers
{
    [Authorize]
    public class MangerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorage;

        public MangerController(IUnitOfWork unitOfWork, IBlobStorageService blobStorage)
        {
            _unitOfWork = unitOfWork;
            _blobStorage = blobStorage;
        }

        public ActionResult Index(string Select)
        {
            var vm = new ProductVM()
            {
                products = _unitOfWork.manger.GetAll(Select),
                episode = _unitOfWork.product.GetEpisode(),
                Shops = _unitOfWork.Shop.GetAll()
            };
            return View(vm);
        }

        [HttpGet]
        public ActionResult CreateSong()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSong([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,ScriptE,ScriptW,CreditsE,CreditsW,Wpdf,Epdf,VideoE,VideoW,ViewsE,ViewsW,WestreanPdfFile,EasternPdfFile")] Product song)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (song.SmallUrl != null)
                    {
                        string ImagePath = await SaveImage(song.SmallUrl);
                        song.SmallImage = ImagePath;
                    }
                    if (song.CoverUrl != null)
                    {
                        string ImagePath = await SaveImage(song.CoverUrl);
                        song.CoverImage = ImagePath;
                    }
                    if (song.WestreanPdfFile != null)
                    {
                        string filename = await SaveFile(song.WestreanPdfFile);
                        song.Wpdf = filename;
                    }
                    if (song.EasternPdfFile != null)
                    {
                        string filename = await SaveFile(song.EasternPdfFile);
                        song.Epdf = filename;
                    }
                    song.Category = Category.Songs;
                    var result = _unitOfWork.manger.Add(song);
                    if (result > 0)
                    {
                        TempData["Messagee"] = song.Title + " " + "has been created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                }
            }
            return View(song);
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBook([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,ScriptE,ScriptW,CreditsE,CreditsW,Link1,Link2,EastrenImageFile,WestreanImageFile,imgUrl3,imgUrl4,imgUrl5,img1,img2,img3,img4,img5")] Product Book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Book.SmallUrl != null)
                    {
                        string ImagePath = await SaveImage(Book.SmallUrl);
                        Book.SmallImage = ImagePath;
                    }
                    if (Book.CoverUrl != null)
                    {
                        string ImagePath = await SaveImage(Book.CoverUrl);
                        Book.CoverImage = ImagePath;
                    }
                    if (Book.imgUrl != null)
                    {
                        string ImagePath = await SaveImage(Book.imgUrl);
                        Book.img1 = ImagePath;
                    }
                    if (Book.imgUrl2 != null)
                    {
                        string ImagePath = await SaveImage(Book.imgUrl2);
                        Book.img2 = ImagePath;
                    }
                    if (Book.imgUrl3 != null)
                    {
                        string ImagePath = await SaveImage(Book.imgUrl3);
                        Book.img3 = ImagePath;
                    }
                    if (Book.imgUrl4 != null)
                    {
                        string ImagePath = await SaveImage(Book.imgUrl4);
                        Book.img4 = ImagePath;
                    }
                    if (Book.imgUrl5 != null)
                    {
                        string ImagePath = await SaveImage(Book.imgUrl5);
                        Book.img5 = ImagePath;
                    }
                    Book.Category = Category.Books;
                    var result = _unitOfWork.manger.Add(Book);
                    if (result > 0)
                    {
                        TempData["Messagee"] = Book.Title + " " + "has been created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    TempData["AlertMessage"] = "Ooops something went wrong !!";
                    return View(Book);
                }
            }
            return View(Book);
        }

        [HttpGet]
        public ActionResult CreateSoftware()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSoftware([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,CreditsE,CreditsW,Link1,Link2,Features1,Features2,Features3,Features4,Features5,Features6,Features7,Features8,EastrenImageFile,WestreanImageFile,imgUrl3,imgUrl4,imgUrl5,img1,img2,img3,img4,img5")] Product soft)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (soft.SmallUrl != null)
                    {
                        string ImagePath = await SaveImage(soft.SmallUrl);
                        soft.SmallImage = ImagePath;
                    }
                    if (soft.CoverUrl != null)
                    {
                        string ImagePath = await SaveImage(soft.CoverUrl);
                        soft.CoverImage = ImagePath;
                    }
                    if (soft.imgUrl != null)
                    {
                        string ImagePath = await SaveImage(soft.imgUrl);
                        soft.img1 = ImagePath;
                    }
                    if (soft.imgUrl2 != null)
                    {
                        string ImagePath = await SaveImage(soft.imgUrl2);
                        soft.img2 = ImagePath;
                    }
                    if (soft.imgUrl3 != null)
                    {
                        string ImagePath = await SaveImage(soft.imgUrl3);
                        soft.img3 = ImagePath;
                    }
                    if (soft.imgUrl4 != null)
                    {
                        string ImagePath = await SaveImage(soft.imgUrl4);
                        soft.img4 = ImagePath;
                    }
                    if (soft.imgUrl5 != null)
                    {
                        string ImagePath = await SaveImage(soft.imgUrl5);
                        soft.img5 = ImagePath;
                    }
                    soft.Category = Category.Software;
                    var result = _unitOfWork.manger.Add(soft);
                    if (result > 0)
                    {
                        TempData["Messagee"] = soft.Title + " " + "has been created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    TempData["AlertMessage"] = "Ooops something went wrong !!";
                    return View(soft);
                }
            }
            return View(soft);
        }

        [HttpGet]
        public ActionResult CreateCartoonSeries()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCartoonSeries([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,VideoE,VideoW,ViewsE,ViewsW")] Product cartoonseries)
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
                    var result = _unitOfWork.manger.Add(cartoonseries);
                    if (result > 0)
                    {
                        TempData["Messagee"] = cartoonseries.Title + " " + "has been created successfully!";
                        return RedirectToAction("Index");
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
        public ActionResult CreateEpisode()
        {
            var prod = new SelectList(_unitOfWork.product.GetAll()?.ToDictionary(p => p.Id, p => p.Title), "Key", "Value");
            ViewBag.ProductId = prod;
            return View(new ProductEpisode());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEpisode(ProductEpisode cartoonepisode)
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

                var result = _unitOfWork.manger.AddEpisode(cartoonepisode);
                if (result > 0)
                {
                    TempData["Messagee"] = cartoonepisode.Title + " " + "has been created successfully!";
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(cartoonepisode);
        }

        [HttpGet]
        public ActionResult EditEpisode(int id)
        {
            var prod = new SelectList(_unitOfWork.product.GetAll()?.ToDictionary(p => p.Id, p => p.Title), "Key", "Value");
            ViewBag.ProductId = prod;
            if (id == 0)
            {
                return NotFound();
            }
            var prodEp = _unitOfWork.product.GetOneEpisode(id);
            if (prodEp == null)
            {
                return NotFound();
            }
            return View(prodEp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEpisode(ProductEpisode episode)
        {
            if (ModelState.IsValid)
            {
                if (episode.EastrenImageFile != null)
                {
                    string oldfile = _unitOfWork.product.GetOneEpisode(episode.Id)?.ImageE ?? string.Empty;
                    if (!string.IsNullOrEmpty(oldfile))
                    {
                        await _blobStorage.DeleteDocumentAsync(oldfile);
                    }
                    var Imagepath = await SaveImage(episode.EastrenImageFile);
                    episode.ImageE = Imagepath;

                }
                if (episode.WestreanImageFile != null)
                {
                    string oldfile = _unitOfWork.product.GetOneEpisode(episode.Id)?.ImageW ?? string.Empty;
                    if (!string.IsNullOrEmpty(oldfile))
                    {
                        await _blobStorage.DeleteDocumentAsync(oldfile);
                    }
                    var Imagepath = await SaveImage(episode.WestreanImageFile);
                    episode.ImageW = Imagepath;
                }
                if (_unitOfWork.manger.EditEP(episode))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(episode);
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
            await _blobStorage.UploadBlobImageAsync(image);
            return ImgName;
        }

        /// <summary>
        /// Save Pdf Files to the Blob storage at PDF Container
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<string> SaveFile(IFormFile file)
        {
            string PathFile = @"https://betkanublob.blob.core.windows.net/betkanublob/PDF/";
            string filename = Path.GetFileNameWithoutExtension(file.FileName);
            string ex = Path.GetExtension(file.FileName);
            string fileName = PathFile + filename + ex;
            await _blobStorage.UploadBlobFileAsync(file);
            return fileName;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var mangerVm = _unitOfWork.product.GetOne(id);
            if (mangerVm == null)
            {
                return NotFound();
            }

            return View(mangerVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Title,SmallImage,CoverImage,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,ScriptE,ScriptW,CreditsE,CreditsW,Link1,Link2,Wpdf,Epdf,VideoE,VideoW,ViewsE,ViewsW,img1,img2,img3,img4,img5,Author,DesignedBy,source,Features1,Features2,Features3,Features4,Features5,Features6,Features7,Features8,SmallUrl,CoverUrl,EastrenImageFile,WestreanImageFile,imgUrl3,imgUrl4,imgUrl5,WestreanPdfFile,EasternPdfFile")] Product prod)
        {
            if (ModelState.IsValid && prod != null)
            {
                try
                {
                    if (prod.SmallUrl != null)
                    {
                        string oldfile = _unitOfWork.product.GetOne(prod.Id)?.SmallImage ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.SmallUrl);
                        prod.SmallImage = Imagepath;
                    }
                    else
                    {
                        string currectImage = _unitOfWork.product?.GetOne(prod.Id)?.SmallImage ?? string.Empty;
                        // If the user did not change the image, keep the old image path
                        prod.SmallImage = prod.SmallImage;
                    }

                    
                    if (prod.CoverUrl != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.CoverImage ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.CoverUrl);
                        prod.CoverImage = Imagepath;
                    }
                    else
                    {
                        string currectCover = _unitOfWork.product?.GetOne(prod.Id)?.CoverImage ?? string.Empty;
                        // If the user did not change the image, keep the old image path
                        prod.CoverImage = currectCover;
                    }

                    if (prod.imgUrl != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.img1 ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.imgUrl);
                        prod.img1 = Imagepath;
                    }
                    else
                    {
                        string currectImage = _unitOfWork.product?.GetOne(prod.Id)?.img1 ?? string.Empty;
                        // If the user did not change the image, keep the old image path
                        prod.img1 = prod.img1;
                    }


                    if (prod.imgUrl2 != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.img2 ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.imgUrl2);
                        prod.img2 = Imagepath;
                    }
                    else
                    {
                        string currectImage = _unitOfWork.product?.GetOne(prod.Id)?.img2 ?? string.Empty;
                        // If the user did not change the image, keep the old image path
                        prod.img2 = prod.img2;
                    }


                    if (prod.imgUrl3 != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.img3 ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.imgUrl3);
                        prod.img3 = Imagepath;
                    }
                    else
                    {
                        string currectImage = _unitOfWork.product?.GetOne(prod.Id)?.img3 ?? string.Empty;
                        // If the user did not change the image, keep the old image path
                        prod.img3 = prod.img3;
                    }


                    if (prod.imgUrl4 != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.img4 ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.imgUrl4);
                        prod.img4 = Imagepath;
                    }
                    else
                    {
                        string currectImage = _unitOfWork.product?.GetOne(prod.Id)?.img4 ?? string.Empty;
                        // If the user did not change the image, keep the old image path
                        prod.img4 = prod.img4;
                    }


                    if (prod.imgUrl5 != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.img5 ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Imagepath = await SaveImage(prod.imgUrl5);
                        prod.img5 = Imagepath;
                    }
                    else
                    {
                        string currectImage = _unitOfWork.product?.GetOne(prod.Id)?.img5 ?? string.Empty;
                        // If the user did not change the image, keep the old image path
                        prod.img5 = prod.img5;
                    }

                    if (prod.WestreanPdfFile != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.Wpdf ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Filepath = await SaveFile(prod.WestreanPdfFile);
                        prod.Wpdf = Filepath;
                    }
                    else
                    {
                        string currectFilepath = _unitOfWork.product?.GetOne(prod.Id)?.Wpdf ?? string.Empty;
                        prod.Wpdf = prod.Wpdf;
                    }

                    if (prod.EasternPdfFile != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.Epdf ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Filepath = await SaveFile(prod.EasternPdfFile);
                        prod.Epdf = Filepath;
                    }
                    else
                    {
                        string currectFilepath = _unitOfWork.product?.GetOne(prod.Id)?.Epdf ?? string.Empty;
                        prod.Epdf = prod.Epdf;
                    }

                    if (_unitOfWork.manger.Edit(prod))
                    {
                        return RedirectToAction(nameof(Index));
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


        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var prod = _unitOfWork.product.GetOne(id);
            if (prod == null)
            {
                return NotFound();
            }
            return View(prod);
        }

        [HttpPost]
        public async Task<ActionResult> DeletePost(int id)
        {
            if (ModelState.IsValid)
            {
                var smallimg = _unitOfWork.product.GetOne(id)?.SmallImage ?? string.Empty;
                if (smallimg != null && smallimg != "")
                {
                    await _blobStorage.DeleteDocumentAsync(smallimg);
                }
                var coverimg = _unitOfWork.product.GetOne(id)?.CoverImage ?? string.Empty;
                if (coverimg != null && coverimg != "")
                {
                    await _blobStorage.DeleteDocumentAsync(coverimg);
                }
                if (_unitOfWork.manger.Delete(id))
                {
                    TempData["Message"] = "The Product has been Delete successfully!";
                }

            }
            return RedirectToAction("Index");
        }




        public ActionResult AddSale()
        {
            return View(new Shop());
        }

        [HttpPost]
        public ActionResult AddSale(Shop shop)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.manger.Add(shop);
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        [HttpGet]
        public ActionResult EditSales(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var s = _unitOfWork.Shop.Getone(id);
            if (s == null)
            {
                return NotFound();
            }
            return View(s);
        }

        [HttpPost]
        public ActionResult EditSales(Shop shop)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.manger.EditShop(shop);
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }
        public ActionResult DeleteSales(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var s = _unitOfWork.Shop.Getone(id);
            if (s == null)
            {
                return NotFound();
            }
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSales(int id)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.manger.DeleteShop(id))
                {
                    TempData["Message"] = "The Sales has been Delete successfully!";
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


    }
}
