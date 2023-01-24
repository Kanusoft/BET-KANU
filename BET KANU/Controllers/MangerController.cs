using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BET_KANU.Services;
using BetKanu.Data;
using System;
using BET_KANU.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BET_KANU.Controllers
{
    [Authorize]
    public class MangerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BKdbContext _bKdb;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly IBlobStorageService _blobStorage;
        public MangerController(IUnitOfWork unitOfWork, IWebHostEnvironment HostEnvironment, IBlobStorageService blobStorage,BKdbContext bKdb)
        {
            _bKdb = bKdb;
            _unitOfWork = unitOfWork;
            _HostEnvironment = HostEnvironment;
            _blobStorage = blobStorage;
        }
        public ActionResult Index(string Select)
        {
            //var Prod = _unitOfWork.manger.GetAll(Select);
            var vm = new ProductVM()
            {
                products = _unitOfWork.manger.GetAll(Select),
                episode = _unitOfWork.product.GetEpisode()
            };
            return View(vm);
        }

        public ActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind("Id,Title,SmallUrl,CoverUrl,imgUrl,imgUrl2,imgUrl3,imgUrl4,imgUrl5,Category,SubCategory,TargetAudince,ReleaseDate,ShortDescription,LongDescription,ScriptE,ScriptW,CreditsE,CreditsW,Link1,Link2,Wpdf,Epdf,VideoE,VideoW,ViewsE,ViewsW")] Product p)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (p.SmallUrl != null)
                    {
                        string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                        string Pathimg = @"/Uploades/";
                        string filename = Path.GetFileNameWithoutExtension(p.SmallUrl.FileName);
                        string ex = Path.GetExtension(p.SmallUrl.FileName);
                        p.SmallImage = Pathimg + filename + ex;
                        filename = filename + ex;
                        string path = Path.Combine(Rootpath, filename);
                        p.SmallUrl.CopyTo(new FileStream(path, FileMode.Create));

                    }
                    if (p.CoverUrl != null)
                    {
                        string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                        string Pathimg = @"/Uploades/";
                        string filename = Path.GetFileNameWithoutExtension(p.CoverUrl.FileName);
                        string ex = Path.GetExtension(p.CoverUrl.FileName);
                        p.CoverImage = Pathimg + filename + ex;
                        filename = filename + ex;
                        string path = Path.Combine(Rootpath, filename);
                        p.CoverUrl.CopyTo(new FileStream(path, FileMode.Create));

                    }
                    if (p.imgUrl != null)
                    {
                        string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                        string Pathimg = @"/Uploades/";
                        string filename = Path.GetFileNameWithoutExtension(p.imgUrl.FileName);
                        string ex = Path.GetExtension(p.imgUrl.FileName);
                        p.img1 = Pathimg + filename + ex;
                        filename = filename + ex;
                        string path = Path.Combine(Rootpath, filename);
                        p.imgUrl.CopyTo(new FileStream(path, FileMode.Create));

                    }
                    if (p.imgUrl2 != null)
                    {
                        string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                        string Pathimg = @"/Uploades/";
                        string filename = Path.GetFileNameWithoutExtension(p.imgUrl2.FileName);
                        string ex = Path.GetExtension(p.imgUrl2.FileName);
                        p.img2 = Pathimg + filename + ex;
                        filename = filename + ex;
                        string path = Path.Combine(Rootpath, filename);
                        p.imgUrl2.CopyTo(new FileStream(path, FileMode.Create));

                    }
                    if (p.imgUrl3 != null)
                    {
                        string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                        string Pathimg = @"/Uploades/";
                        string filename = Path.GetFileNameWithoutExtension(p.imgUrl3.FileName);
                        string ex = Path.GetExtension(p.imgUrl3.FileName);
                        p.img3 = Pathimg + filename + ex;
                        filename = filename + ex;
                        string path = Path.Combine(Rootpath, filename);
                        p.imgUrl3.CopyTo(new FileStream(path, FileMode.Create));

                    }
                    if (p.imgUrl4 != null)
                    {
                        string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                        string Pathimg = @"/Uploades/";
                        string filename = Path.GetFileNameWithoutExtension(p.imgUrl4.FileName);
                        string ex = Path.GetExtension(p.imgUrl4.FileName);
                        p.img4 = Pathimg + filename + ex;
                        filename = filename + ex;
                        string path = Path.Combine(Rootpath, filename);
                        p.imgUrl4.CopyTo(new FileStream(path, FileMode.Create));

                    }
                    if (p.imgUrl5 != null)
                    {
                        string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                        string Pathimg = @"/Uploades/";
                        string filename = Path.GetFileNameWithoutExtension(p.imgUrl5.FileName);
                        string ex = Path.GetExtension(p.imgUrl5.FileName);
                        p.img5 = Pathimg + filename + ex;
                        filename = filename + ex;
                        string path = Path.Combine(Rootpath, filename);
                        p.imgUrl5.CopyTo(new FileStream(path, FileMode.Create));
                    }
                    TempData["Message"] = p.Title + " " + "has been created successfully!";
                    _unitOfWork.manger.Add(p);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                }
            }
            return View(p);
        }
        private string SaveImage(IFormFile file)
        {
            //WebImage img = new WebImage(file.ContentDisposition);
            //string webRoot = _HostEnvironment.WebRootPath;
            //string? getFileName;
            //getFileName = file.FileName;
            //var Filename = Path.GetFileName(file.FileName);
            //string newFileName = name.ToString() + Filename.Substring(Filename.LastIndexOf("."), 4);
            //var path = Path.Combine((isCompletePath ? Serverpath : webRoot), newFileName);
            //img.Save(path);
            string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
            string fullpath = Path.Combine(uploade, file.FileName);
            file.CopyTo(new FileStream(fullpath, FileMode.Create));

            return file.FileName;
        }
        public ActionResult Edit(int id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var mangerVm = _unitOfWork.manger.GetProductInfo(id);
            if(mangerVm == null)
            {
                return NotFound();
            }
            
            return View(mangerVm);
        }

        [HttpPost]
        public ActionResult Edit(MangerVM manger)
        {
            if (ModelState.IsValid)
            {               
                string filename = string.Empty;
                if (manger.product.SmallUrl != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename = manger.product.SmallUrl.FileName;
                    string fullpath = Path.Combine(uploade, filename);

                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOne(manger.product.Id).SmallImage;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if(fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        manger.product.SmallUrl.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                string filename2 = string.Empty;
                if (manger.product.CoverUrl != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename2 = manger.product.CoverUrl.FileName;
                    string fullpath = Path.Combine(uploade, filename2);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOne(manger.product.Id).CoverImage;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        manger.product.CoverUrl.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                string filename3 = string.Empty;
                if (manger.product.imgUrl != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename3 = manger.product.imgUrl.FileName;
                    string fullpath = Path.Combine(uploade, filename3);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOne(manger.product.Id).img1;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        manger.product.imgUrl.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                string filename4 = string.Empty;
                if (manger.product.imgUrl2 != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename4 = manger.product.imgUrl2.FileName;
                    string fullpath = Path.Combine(uploade, filename4);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOne(manger.product.Id).img2;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        manger.product.imgUrl2.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                string filename5 = string.Empty;
                if (manger.product.imgUrl3 != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename5 = manger.product.imgUrl3.FileName;
                    string fullpath = Path.Combine(uploade, filename5);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOne(manger.product.Id).img3;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        manger.product.imgUrl3.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                string filename6 = string.Empty;
                if (manger.product.imgUrl4 != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename6 = manger.product.imgUrl4.FileName;
                    string fullpath = Path.Combine(uploade, filename6);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOne(manger.product.Id).img4;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        manger.product.imgUrl4.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                string filename7 = string.Empty;
                if (manger.product.imgUrl5 != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename5 = manger.product.imgUrl5.FileName;
                    string fullpath = Path.Combine(uploade, filename5);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOne(manger.product.Id).img5;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        manger.product.imgUrl5.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                if (_unitOfWork.manger.Edit(manger))
                {
                    return RedirectToAction(nameof(Index));
                }                             
            }
            return View(manger);
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
                TempData["Message"] =  "The Product has been Delete successfully!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateEpisode()
        {
            var prod = new SelectList(_unitOfWork.product.GetAll()?.ToDictionary(p => p.Id,p=>p.Title), "Key", "Value");
            ViewBag.ProductId = prod;
            return View(new ProductEpisode());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEpisode(ProductEpisode episode)
        {
            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (episode.imgUrl != null)
                {
                    string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades/Episode");
                    string Pathimg = @"/Uploades/Episode/";
                    filename = Path.GetFileNameWithoutExtension(episode.imgUrl.FileName);
                    string ex = Path.GetExtension(episode.imgUrl.FileName);
                    episode.ImageE = Pathimg + filename + ex;
                    filename = filename + ex;
                    string path = Path.Combine(Rootpath, filename);
                    episode.imgUrl.CopyTo(new FileStream(path, FileMode.Create));
                }
                string filename2 = string.Empty;
                if (episode.imgUrl2 != null)
                {
                    string Rootpath = Path.Combine(_HostEnvironment.WebRootPath, "Uploades/Episode");
                    string Pathimg = @"/Uploades/Episode/";
                    filename2 = Path.GetFileNameWithoutExtension(episode.imgUrl2.FileName);
                    string ex = Path.GetExtension(episode.imgUrl2.FileName);
                    episode.ImageW = Pathimg + filename + ex;
                    filename = filename + ex;
                    string path = Path.Combine(Rootpath, filename);
                    episode.imgUrl2.CopyTo(new FileStream(path, FileMode.Create));
                }
                _unitOfWork.manger.Add(episode);
                return RedirectToAction(nameof(Index));
            }          
            return View(episode);
        }
        public ActionResult EditEpisode(int id)
        {
            var prod = new SelectList(_unitOfWork.product.GetAll()?.ToDictionary(p => p.Id, p => p.Title), "Key", "Value");
            ViewBag.ProductId = prod;
            if (id == null || id == 0)
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
        public ActionResult EditEpisode(ProductEpisode episode)
        {
            if (ModelState.IsValid)
            {       
                string filename3 = string.Empty;
                if (episode.imgUrl != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename3 = episode.imgUrl.FileName;
                    string fullpath = Path.Combine(uploade, filename3);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOneEpisode(episode.Id).ImageE;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        episode.imgUrl.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                string filename4 = string.Empty;
                if (episode.imgUrl2 != null)
                {
                    string uploade = Path.Combine(_HostEnvironment.WebRootPath, "Uploades");
                    filename4 = episode.imgUrl2.FileName;
                    string fullpath = Path.Combine(uploade, filename4);
                    //Delete The Old File
                    string oldFile = _unitOfWork.product.GetOneEpisode(episode.Id).ImageW;
                    string OldPath = Path.Combine(uploade, oldFile);
                    if (fullpath != OldPath)
                    {
                        System.IO.File.Delete(OldPath);
                        //Save The New File
                        episode.imgUrl2.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }            
            }
                if (_unitOfWork.manger.EditEP(episode))
                {
                    return RedirectToAction(nameof(Index));
                }
            return View(episode);
        }
                    
        public ActionResult DeleteEpisode(int id)
        {
            if (id == null || id == 0)
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
        public ActionResult DeleteEPpost(int id)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.manger.DeleteEP(id);
                TempData["Message"] = "The Product has been Delete successfully!";
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
