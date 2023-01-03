using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BET_KANU.Services;

namespace BET_KANU.Controllers
{
    [Authorize]
    public class MangerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBlobStorageService _blobStorage;
        public MangerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IBlobStorageService blobStorage)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _blobStorage = blobStorage;
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
        public async Task <ActionResult> Create(Product product,IFormFile SmallImage, IFormFile CoverImage , IFormFile image1 , IFormFile image2, IFormFile image3, IFormFile image4, IFormFile image5)
        {
            if (ModelState.IsValid)
            {
                if(SmallImage != null)
                {
                    var Serverpath = @"/img/Product/";
                    var filename = $"{product.Id}_{product.Title}";
                    //string newfile = SaveImage(SmallImage, Serverpath, filename, false);
                    //product.SmallImage = Serverpath + filename + SmallImage.FileName.Substring(SmallImage.FileName.LastIndexOf('.'),4);
                    await _blobStorage.UploadBlobFileAsync(SmallImage);
                    product.SmallImage = Serverpath + filename + SmallImage.FileName.Substring(SmallImage.FileName.LastIndexOf('.'), 4);
                }
                if(CoverImage != null)
                {
                    var Serverpath = @"/img/Product/";
                    var filename = $"{product.Id}_{product.Title}";
                    //string newfile = SaveImage(CoverImage, Serverpath, filename, false);
                    //product.CoverImage = Serverpath + filename + CoverImage.FileName.Substring(CoverImage.FileName.LastIndexOf('.'), 4);
                    await _blobStorage.UploadBlobFileAsync(CoverImage);
                    product.CoverImage = Serverpath + filename + CoverImage.FileName.Substring(CoverImage.FileName.LastIndexOf('.'), 4);
                }
                if(image1 != null)
                {
                    var Serverpath = @"/img/Product/";
                    var filename = $"{product.Id}_{product.Title}";
                    await _blobStorage.UploadBlobFileAsync(image1);
                    product.img1 = Serverpath + filename + image1.FileName.Substring(image1.FileName.LastIndexOf('.'), 4);
                }
                if (image2 != null)
                {
                    var Serverpath = @"/img/Product/";
                    var filename = $"{product.Id}_{product.Title}";
                    await _blobStorage.UploadBlobFileAsync(image2);
                    product.img2 = Serverpath + filename + image2.FileName.Substring(image2.FileName.LastIndexOf('.'), 4);
                }
                if (image3 != null)
                {
                    var Serverpath = @"/img/Product/";
                    var filename = $"{product.Id}_{product.Title}";
                    await _blobStorage.UploadBlobFileAsync(image3);
                    product.img3 = Serverpath + filename + image3.FileName.Substring(image3.FileName.LastIndexOf('.'), 4);
                }
                if (image4 != null)
                {
                    var Serverpath = @"/img/Product/";
                    var filename = $"{product.Id}_{product.Title}";
                    await _blobStorage.UploadBlobFileAsync(image4);
                    product.img4 = Serverpath + filename + image4.FileName.Substring(image4.FileName.LastIndexOf('.'), 4);
                }
                if (image5 != null)
                {
                    var Serverpath = @"/img/Product/";
                    var filename = $"{product.Id}_{product.Title}";
                    await _blobStorage.UploadBlobFileAsync(image5);
                    product.img5 = Serverpath + filename + image5.FileName.Substring(image5.FileName.LastIndexOf('.'), 4);
                }
                _unitOfWork.manger.Add(product);
            }
            return View(product);
        }
        //private string SaveImage(IFormFile file, string Serverpath, string name,bool isCompletePath)
        //{
        //    WebImage img = new WebImage(file.ContentDisposition);
        //    string webRoot = _webHostEnvironment.WebRootPath;
        //    string? getFileName;
        //    getFileName = file.FileName;
        //    var Filename = Path.GetFileName(file.FileName);
        //    string newFileName = name.ToString() + Filename.Substring(Filename.LastIndexOf("."), 4);
        //    var path = Path.Combine((isCompletePath ? Serverpath : webRoot), newFileName);
        //    img.Save(path);

        //    return newFileName;
        //}
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
        public async Task<ActionResult> Edit(Product product, IFormFile SmallImage, IFormFile CoverImage, IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4, IFormFile image5)
        {
            if (ModelState.IsValid)
            {
                if (SmallImage != null)
                {
                    //var Serverpath = @"/img/Product/";
                    //var filename = $"{product.Id}_{product.Title}";
                    //string newfile = SaveImage(SmallImage, Serverpath, filename, false);
                    await _blobStorage.UploadBlobFileAsync(SmallImage);
                    product.SmallImage = SmallImage.FileName.Substring(SmallImage.FileName.LastIndexOf('.'), 4);
                }
                if (CoverImage != null)
                {
                    //var Serverpath = @"/img/Product/";
                    //var filename = $"{product.Id}_{product.Title}";
                    //string newfile = SaveImage(CoverImage, Serverpath, filename, false);
                    await _blobStorage.UploadBlobFileAsync(CoverImage);
                    product.CoverImage = CoverImage.FileName.Substring(CoverImage.FileName.LastIndexOf('.'), 4);
                }
                if (image1 != null)
                {
                    await _blobStorage.UploadBlobFileAsync(image1);
                    product.img1 = image1.FileName.Substring(image1.FileName.LastIndexOf('.'), 4);
                }
                if (image2 != null)
                {
                    await _blobStorage.UploadBlobFileAsync(image2);
                    product.img2 = image2.FileName.Substring(image2.FileName.LastIndexOf('.'), 4);
                }
                if (image3 != null)
                {
                    await _blobStorage.UploadBlobFileAsync(image3);
                    product.img3 = image3.FileName.Substring(image3.FileName.LastIndexOf('.'), 4);
                }
                if (image4 != null)
                {
                    await _blobStorage.UploadBlobFileAsync(image4);
                    product.img4 = image4.FileName.Substring(image4.FileName.LastIndexOf('.'), 4);
                }
                if (image5 != null)
                {
                    await _blobStorage.UploadBlobFileAsync(image5);
                    product.img5 = image5.FileName.Substring(image5.FileName.LastIndexOf('.'), 4);
                }
                _unitOfWork.manger.Edit(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

       
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
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
        public ActionResult DeletePost(int id)
        {
            if (ModelState.IsValid)
            {
                var smallimg = _unitOfWork.product.GetOne(id).SmallImage;
                if (smallimg != null)
                {
                    FileInfo file = new FileInfo(smallimg);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                var coverimg = _unitOfWork.product.GetOne(id).CoverImage;
                if (coverimg != null)
                {
                    FileInfo file = new FileInfo(coverimg);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                _unitOfWork.manger.Delete(id);
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
