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
        public async Task <ActionResult> Create(Product product,IFormFile SmallImage, IFormFile CoverImage)
        {
            if (ModelState.IsValid)
            {
                if(SmallImage != null)
                {
                    //var Serverpath = @"/img/Product/";
                    //var filename = $"{product.Id}_{product.Title}";
                    //string newfile = SaveImage(SmallImage, Serverpath, filename, false);
                    //product.SmallImage = Serverpath + filename + SmallImage.FileName.Substring(SmallImage.FileName.LastIndexOf('.'),4);
                    await _blobStorage.UploadBlobFileAsync(SmallImage);
                    product.SmallImage = SmallImage.FileName.Substring(SmallImage.FileName.LastIndexOf('.'), 4);
                }
                if(CoverImage != null)
                {
                    //var Serverpath = @"/img/Product/";
                    //var filename = $"{product.Id}_{product.Title}";
                    //string newfile = SaveImage(CoverImage, Serverpath, filename, false);
                    //product.CoverImage = Serverpath + filename + CoverImage.FileName.Substring(CoverImage.FileName.LastIndexOf('.'), 4);
                    await _blobStorage.UploadBlobFileAsync(CoverImage);
                    product.CoverImage = CoverImage.FileName.Substring(CoverImage.FileName.LastIndexOf('.'), 4);
                }
                _unitOfWork.manger.Add(product);
            }
            return View(product);
        }
        private string SaveImage(IFormFile file, string Serverpath, string name,bool isCompletePath)
        {
            WebImage img = new WebImage(file.ContentDisposition);
            string webRoot = _webHostEnvironment.WebRootPath;
            string? getFileName;
            getFileName = file.FileName;
            var Filename = Path.GetFileName(file.FileName);
            string newFileName = name.ToString() + Filename.Substring(Filename.LastIndexOf("."), 4);
            var path = Path.Combine((isCompletePath ? Serverpath : webRoot), newFileName);
            img.Save(path);

            return newFileName;
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
        public async Task<ActionResult> Edit(Product product, IFormFile SmallImage, IFormFile CoverImage)
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
