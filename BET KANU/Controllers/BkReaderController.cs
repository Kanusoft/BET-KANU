using BetKanu.Models.Interface;
using BetKanu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BkReaderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BkReaderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/BKReader
        [HttpGet]
        public ActionResult Get(int? bookId, int? page, int? sec, int? chapterNo)
        {
            if (Request.Headers.TryGetValue("FROM", out var headervalue))
            {
                if (headervalue == "BETKANU")
                {
                    if (bookId.HasValue)
                    {
                        if (sec == null)
                        {
                            sec = 1;
                        }
                        if (page.HasValue && sec.HasValue)
                        {
                            var bundle = _unitOfWork.bKBundle.GetBundle(bookId.Value, page.Value, sec.Value);
                            return Ok(bundle);
                        }
                        else if (chapterNo.HasValue)
                        {
                            var bundles = _unitOfWork.bKBundle.GetBundles(bookId.Value, chapterNo.Value);
                            return Ok(bundles);
                        }
                        else
                        {
                            // Handle the request with bookId parameter only
                            var bundel = _unitOfWork.bKBundle.GetBundles(bookId.Value);
                            if (bundel == null)
                            {
                                return NotFound();
                            }
                            return Ok(bundel.FirstOrDefault());
                        }
                    }
                    else
                    {
                        // Handle the request without any parameters
                        var books = _unitOfWork.bKBundle.GetBooks();
                        return Ok(books);
                    }
                }
                else
                {
                    return Redirect("https://betkanu.com/home/reader");
                }
            }
            else 
            {
                return Redirect("https://betkanu.com/home/reader");
            }
          
        }
    }
}
