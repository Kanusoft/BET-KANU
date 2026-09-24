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
            // Backward compatibility for malformed QR URLs such as "?page11"
            // (missing "="), which ASP.NET does not bind to the page parameter.
            if (!page.HasValue)
            {
                page = GetPageFromMalformedQuery();
            }

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

        /// <summary>
        /// Parses malformed query keys like "page11" / "PAGE11" into a page number.
        /// Only matches keys that are exactly "page" followed by digits.
        /// </summary>
        private int? GetPageFromMalformedQuery()
        {
            foreach (var key in Request.Query.Keys)
            {
                if (key is null
                    || !key.StartsWith("page", StringComparison.OrdinalIgnoreCase)
                    || key.Length <= 4)
                {
                    continue;
                }

                var pagePart = key.Substring(4);

                if (pagePart.All(char.IsDigit)
                    && int.TryParse(pagePart, out var parsedPage)
                    && parsedPage > 0)
                {
                    return parsedPage;
                }
            }

            return null;
        }
    }
}
