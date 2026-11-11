using BetKanu.Models;
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
                        var books = _unitOfWork.bKBundle.GetBooks().Where(b =>
                        b.Id != 27 &&
                        b.Id != 28)
                    .ToList();

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
        /// BK Reader V2 endpoint.
        /// Without bookId: Returns the available books list.
        /// With bookId and page: Returns the scanned bundle with
        /// its immediate previous and next bundles.
        /// Requires FROM: BETKANU header.
        /// </summary>
        [HttpGet("v2")]
        public async Task<IActionResult> GetV2(
            [FromQuery] int? bookId,
            [FromQuery] int? page,
            [FromQuery] int? sec = null,
            [FromQuery] int? chapterNo = null,
            CancellationToken cancellationToken = default)
        {
            // Validate the FROM header (same behavior as V1).
            if (!Request.Headers.TryGetValue("FROM", out var headerValue) ||
                !string.Equals(
                    headerValue.ToString(),
                    "BETKANU",
                    StringComparison.OrdinalIgnoreCase))
            {
                return Redirect("https://betkanu.com/home/reader");
            }

            // No bookId: Return the books list (same as V1).
            if (!bookId.HasValue)
            {
                var books = _unitOfWork.bKBundle.GetBooks()
                    .Where(b =>
                        b.Id != 27 &&
                        b.Id != 28)
                    .ToList();

                if (books.Count == 0)
                    return BadRequest("No Content");

                return Ok(books);
            }

            // Validate bookId.
            if (bookId.Value <= 0)
                return BadRequest("A positive bookId is required.");

            // Keep compatibility with malformed QR codes.
            // Example: ?page11 instead of ?page=11.
            page ??= GetPageFromMalformedQuery();

            if (!page.HasValue || page.Value <= 0)
                return BadRequest("A positive page is required.");

            // Default section number.
            int sectionNumber = sec ?? 1;

            if (sectionNumber <= 0)
                return BadRequest("sec must be a positive number.");

            try
            {
                var window = await _unitOfWork.bKBundle.GetReaderWindowAsync(
                    bookId.Value,
                    page.Value,
                    sectionNumber,
                    chapterNo,
                    cancellationToken);

                if (window is null)
                {
                    return BadRequest(
                        "Released book or requested content not found.");
                }

                return Ok(window);
            }
            catch (BkReaderAmbiguousPageException ex)
            {
                return BadRequest(new { message = ex.Message });
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
