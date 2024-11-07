using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BET_KANU.Services;
using BET_KANU.ViewModels;
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
        public async Task<ActionResult> CreateSong([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,ScriptE,ScriptW,CreditsE,CreditsW,Wpdf,Epdf,VideoE,VideoW,ViewsE,ViewsW,WestreanPdfFile,EasternPdfFile,IsRelease,MalouliScript,EnglishScript,CreditsM,VideoM,ViewsM,MalouliPdfFile")] Product song)
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
                    if (song.MalouliPdfFile != null)
                    {
                        string filename = await SaveFile(song.MalouliPdfFile);
                        song.Mpdf = filename;
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
        public async Task<ActionResult> CreateBook([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,ScriptE,ScriptW,CreditsE,CreditsW,Author,DesignedBy,Features,ProductBy,Created,Link3,SongsList,Info1,EastrenImageFile,WestreanImageFile,imgUrl3,imgUrl4,imgUrl5,img1,img2,img3,img4,img5,IsRelease")] Product Book)
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
        public async Task<ActionResult> CreateSoftware([Bind("Title,SmallImage,CoverImage,SmallUrl,CoverUrl,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,CreditsE,CreditsW,Source,SponsoredBY,Link1,Link2,Features,ProductBy,Created,Link3,SongsList,Info1,Info2,Info3,EastrenImageFile,WestreanImageFile,imgUrl,imgUrl2,imgUrl3,imgUrl4,imgUrl5,img1,img2,img3,img4,img5,IsRelease")] Product soft)
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
                return RedirectToAction("Index");
            }
            var mangerVm = _unitOfWork.product.GetOne(id);
            if (mangerVm == null)
            {
                return RedirectToAction("Index");
            }

            return View(mangerVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Title,SmallImage,CoverImage,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,Source,SponsoredBY,,ScriptE,ScriptW,CreditsE,CreditsW,Link1,Link2,Wpdf,Epdf,VideoE,VideoW,ViewsE,ViewsW,img1,img2,img3,img4,img5,Author,DesignedBy,source,Features,ProductBy,Created,Link3,SongsList,Info1,Info2,Info3,SmallUrl,CoverUrl,EastrenImageFile,WestreanImageFile,imgUrl,imgUrl2,imgUrl3,imgUrl4,imgUrl5,WestreanPdfFile,EasternPdfFile,IsRelease,MalouliScript,EnglishScript,CreditsM,VideoM,ViewsM,MalouliPdfFile")]Product prod)
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
                        prod.SmallImage = currectImage;
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
                        prod.img1 = currectImage;
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
                        prod.img2 = currectImage;
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
                        prod.img3 = currectImage;
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
                        prod.img4 = currectImage;
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
                        prod.img5 = currectImage;
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
                        prod.Wpdf = currectFilepath;
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
                        prod.Epdf = currectFilepath;
                    }

                    if (prod.MalouliPdfFile != null)
                    {
                        string oldfile = _unitOfWork.product?.GetOne(prod.Id)?.Mpdf ?? string.Empty;
                        if (!string.IsNullOrEmpty(oldfile))
                        {
                            await _blobStorage.DeleteDocumentAsync(oldfile);
                        }
                        var Filepath = await SaveFile(prod.MalouliPdfFile);
                        prod.Mpdf = Filepath;
                    }
                    else
                    {
                        string currectFilepath = _unitOfWork.product?.GetOne(prod.Id)?.Mpdf ?? string.Empty;
                        prod.Mpdf = currectFilepath;
                    }

                    if (_unitOfWork.manger.Edit(prod))
                    {
                        TempData["Messagee"] = prod.Title + " " + "has been Updated successfully!";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePost(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            var prod = _unitOfWork.product.GetOne(id);
            if (prod == null)
            {
                return RedirectToAction("Index");
            }
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

        [HttpGet]
        public ActionResult AddSale()
        {
            return View(new Shop());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSale([Bind("Name,ImageUrl,Price,ReleaseDate,FileUrl")]Shop shop)
        {
            if (ModelState.IsValid && shop != null)
            {
                if (shop.FileUrl != null)
                {
                    shop.ImageUrl = await SaveImage(shop.FileUrl);
                }
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
                return RedirectToAction("Index");
            }
            var s = _unitOfWork.Shop.Getone(id);
            if (s == null)
            {
                return RedirectToAction("Index");
            }
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSales([Bind("Name,ImageUrl,Price,ReleaseDate,FileUrl")] Shop shop)
        {
            if (ModelState.IsValid)
            {

                if (shop.FileUrl != null)
                {
                    string oldfile = _unitOfWork.Shop.Getone(shop.Id).ImageUrl ?? string.Empty;
                    if (!string.IsNullOrEmpty(oldfile))
                    {
                        await _blobStorage.DeleteDocumentAsync(oldfile);
                    }
                    var Imagepath = await SaveImage(shop.FileUrl);
                    shop.ImageUrl = Imagepath;
                }
                else
                {
                    string currectImage = _unitOfWork.Shop.Getone(shop.Id).ImageUrl ?? string.Empty;
                    shop.ImageUrl = currectImage;
                }

                if (_unitOfWork.manger.EditShop(shop))
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSales(int id)
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
            if (ModelState.IsValid)
            {
                if (_unitOfWork.manger.DeleteShop(id))
                {
                    TempData["Message"] = "The Sales has been Delete successfully!";
                }
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult PreviewProduct(int id)
        {
            var vm = new ProductVM();
            vm.product = _unitOfWork.product.GetOne(id);
        

            vm.WestrenEpisodes = _unitOfWork.product.GetByParentIdandLang(id, Language.Westren);
            vm.EastrenEpisodes = _unitOfWork.product.GetByParentIdandLang(id, Language.Eastren);

            return View(vm);
        }

        [HttpPost]
        public ActionResult UpdateIsRelease(int id, bool isRelease)
        {
            var product = _unitOfWork.product.GetOne(id);
            if (product != null)
            {
                product.IsRelease = isRelease;
                _unitOfWork.manger.Edit(product);
                return Json(new { success = true, message = "Update successful" });
            }
            return Json(new { success = false, message = "Product not found" });
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


    }
}
