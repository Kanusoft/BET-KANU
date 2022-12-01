using BetKanu.Models.Interface;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BET_KANU.Controllers
{
    public class BKReaderServiceController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public BKReaderServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/BKReader
        /// <summary>
        /// Get list of books.
        /// book image-short explanation about book- download pdf button - download book offline - see the book in webpage)
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            //Here i need to redirect to a aspx page.
            Request.Headers.TryGetValues("FROM", out IEnumerable<string>? from);
            var headerValue = from?.FirstOrDefault();
            if (from != null && headerValue == "BETKANU")
            {
                var books = _unitOfWork.bKBundle.GetBooks();

                var response = Request.CreateResponse(HttpStatusCode.OK, books);

                return response;
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri("https://betkanu.com/home/reader");
                return response;
            }
        }

        // GET api/BKReader?bookid=6
        public HttpResponseMessage Get(int bookId)
        {
            //Here i need to redirect to a aspx page.
            Request.Headers.TryGetValues("FROM", out IEnumerable<string>? from);
            var headerValue = from?.FirstOrDefault();

            if (from != null && headerValue == "BETKANU")
            {
                var bundles = _unitOfWork.bKBundle.GetBundles(bookId);

                var response = Request.CreateResponse(HttpStatusCode.OK, bundles.FirstOrDefault());

                return response;
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri("https://betkanu.com/home/reader");
                return response;
            }
        }

        // GET api/BKReader?bookid=6&chapterNo
        public HttpResponseMessage Get(int bookId, int chapterNo, bool IsTemp)
        {
            //Here i need to redirect to a aspx page.
            Request.Headers.TryGetValues("FROM", out IEnumerable<string>? from);
            var headerValue = from?.FirstOrDefault();
            if (from != null && headerValue == "BETKANU")
            {

                var bundles = _unitOfWork.bKBundle.GetBundles(bookId, chapterNo);

                var response = Request.CreateResponse(HttpStatusCode.OK, bundles);

                return response;
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri("https://betkanu.com/home/reader");
                return response;
            }
        }

        // GET api/BKReader?bookid=6&page=1&sec=4
        public HttpResponseMessage Get(int bookId, int page, int sec = 1)
        {
            //Here i need to redirect to a aspx page.
            Request.Headers.TryGetValues("FROM", out IEnumerable<string>? from);
            var headerValue = from?.FirstOrDefault();
            if (from != null && headerValue == "BETKANU")
            {

                var bundle = _unitOfWork.bKBundle.GetBundle(bookId, page, sec);

                bundle.TextLanguage = bundle.TextLanguage;

                var response = Request.CreateResponse(HttpStatusCode.OK, bundle);

                return response;
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri("https://betkanu.com/home/reader");
                return response;
            }
        }
    }
}
