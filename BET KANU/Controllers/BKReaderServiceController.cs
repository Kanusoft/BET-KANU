using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    [Route("api/BkReader")]
    [ApiController]
    public class BkReaderServiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BkReaderServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/BKReader
        [HttpGet]
        public ActionResult Get(int? bookId, int? page, int? sec , int? chapterNo, int? pageNavigation = 0)
        {
            if (Request.Headers.TryGetValue("FROM", out var headervalue))
            {
                if (headervalue == "BETKANU")
                {
                    if (bookId.HasValue)
                    {
                        if(sec == null)
                        {
                            sec = 1;
                        }

                        if (page.HasValue && sec.HasValue && pageNavigation.HasValue)
                        {
                            var bundle = _unitOfWork.bKBundle.GetBundle(bookId.Value, page.Value, sec.Value, pageNavigation.Value) ?? null;

                            if(bundle is null)
                                return BadRequest("No Content");                           

                            return Ok(bundle);
                        }
                        else if (chapterNo.HasValue)
                        {
                            var bundles = _unitOfWork.bKBundle.GetBundles(bookId.Value, chapterNo.Value) ?? null;

                            if (bundles is null)
                                return BadRequest("No Content");

                            return Ok(bundles);
                        }
                        else
                        {
                            // Handle the request with bookId parameter only
                            var bundle = _unitOfWork.bKBundle.GetBundles(bookId.Value);

                            if(bundle.Count() == 0)
                                return BadRequest("No Content");

                            return Ok(bundle.FirstOrDefault());
                        }
                    }
                    else
                    {
                        // Handle the request without any parameters
                        var books = _unitOfWork.bKBundle.GetBooks();

                        if(books.Count() == 0)
                            return BadRequest("No Content");

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
                var response = new RedirectResult("https://betkanu.com/home/reader");
                return response;
               // return Redirect("https://betkanu.com/home/reader");
            }
          
        }
    }
}
