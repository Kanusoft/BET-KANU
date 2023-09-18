using Azure;
using BetKanu.Models;
using BetKanu.Models.Interface;

namespace BetKanu.Data.Repositories
{
    public class BookBundleDAO : IBKBundle
    {
        private readonly BKdbContext _db;

        public BookBundleDAO(BKdbContext db)
        {
            _db = db;
        }

        public Book GetBook(int id)
        {
            return _db.Books.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Book> GetBooks()
        {
            var books = _db.Books.Where(b => b.IsReleased == true);
            return books.ToList();
        }

        public Bundle? GetBundle(int? id)
        {
            return _db.Bundles.FirstOrDefault(b => b.Id == id);
        }

        public BKRBundle? GetBundle(int? BookId, int? pageNo, int? SecNo, int? pageNavigation )
        {
            bool isFirstPageAndSection = false;
            bool isLastPageAndSection = false;
            //Get Previous QR Bundle 
            if (pageNavigation is -1)
            {
                isFirstPageAndSection = IstherePreviousPage(BookId, pageNo, SecNo);
                isLastPageAndSection = IsThereNextPage(BookId, pageNo, SecNo);
                (int newPageNo, int newSecNo, isFirstPageAndSection) = GetPreviousPage(BookId ?? 0, pageNo ?? 0, SecNo ?? 0, pageNavigation ?? 0);

                var b = from bundle in _db.Bundles
                        join book in _db.Books on bundle.BookId equals book.Id
                        where book.Id == BookId
                        && bundle.PageNo == newPageNo
                        && bundle.SecNo == newSecNo
                        select new BKRBundle()
                        {
                            Book = new()
                            {
                                Name = book.Name,
                            },
                            AudioURL = book.URL + bundle.AudioURL,
                            ExternalURL = bundle.ExternalURL,
                            ExternalURLName = bundle.ExternalURLName,
                            ExternalVideo = bundle.ExternalVideo,
                            ImageURL = book.URL + bundle.ImageURL,
                            ExternalVideoName = bundle.ExternalVideoName,
                            InternalVideo = bundle.InternalVideo,
                            TextLanguage = bundle.TextLanguage.Trim(),
                            TextURL = book.URL + bundle.TextURL,
                            VideoURL = book.URL + bundle.VideoURL,
                            IsFirst = isFirstPageAndSection,
                            IsLast = isLastPageAndSection,
                        };
                return b.FirstOrDefault();

            }
            //Get the Current QR Page
            else if (pageNavigation is 0)
            {
                isFirstPageAndSection = IstherePreviousPage(BookId, pageNo,SecNo);
                isLastPageAndSection = IsThereNextPage(BookId, pageNo, SecNo);

                var b = from bundle in _db.Bundles
                        join book in _db.Books on bundle.BookId equals book.Id
                        where book.Id == BookId
                        && bundle.PageNo == pageNo
                        && bundle.SecNo == SecNo
                        select new BKRBundle()
                        {
                            Book = new()
                            {
                                Name = book.Name,
                            },
                            AudioURL = book.URL + bundle.AudioURL,
                            ExternalURL = bundle.ExternalURL,
                            ExternalURLName = bundle.ExternalURLName,
                            ExternalVideo = bundle.ExternalVideo,
                            ImageURL = book.URL + bundle.ImageURL,
                            ExternalVideoName = bundle.ExternalVideoName,
                            InternalVideo = bundle.InternalVideo,
                            TextLanguage = bundle.TextLanguage.Trim(),
                            TextURL = book.URL + bundle.TextURL,
                            VideoURL = book.URL + bundle.VideoURL,
                            IsFirst = isFirstPageAndSection,
                            IsLast = isLastPageAndSection,
                        };

                return b.FirstOrDefault();
            }
            //Get The Next QR Page
            else if (pageNavigation is 1)
            {
                isFirstPageAndSection = IstherePreviousPage(BookId, pageNo, SecNo);
                isLastPageAndSection = IsThereNextPage(BookId, pageNo, SecNo);
                (int newPageNo, int newSecNo, isLastPageAndSection) = GetNextPage(BookId ?? 0, pageNo ?? 0, SecNo ?? 0, pageNavigation ?? 0);
                var b = from bundle in _db.Bundles
                        join book in _db.Books on bundle.BookId equals book.Id
                        where book.Id == BookId
                        && bundle.PageNo == newPageNo
                        && bundle.SecNo == newSecNo
                        select new BKRBundle()
                        {
                            Book = new()
                            {
                                Name = book.Name,
                            },
                            AudioURL = book.URL + bundle.AudioURL,
                            ExternalURL = bundle.ExternalURL,
                            ExternalURLName = bundle.ExternalURLName,
                            ExternalVideo = bundle.ExternalVideo,
                            ImageURL = book.URL + bundle.ImageURL,
                            ExternalVideoName = bundle.ExternalVideoName,
                            InternalVideo = bundle.InternalVideo,
                            TextLanguage = bundle.TextLanguage.Trim(),
                            TextURL = book.URL + bundle.TextURL,
                            VideoURL = book.URL + bundle.VideoURL,
                            IsFirst = isFirstPageAndSection,
                            IsLast = isLastPageAndSection,
                        };
                return b.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Get the Current Previous QR Page and Section
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="pageNo"></param>
        /// <param name="secNo"></param>
        /// <param name="pageNavigation"></param>
        /// <returns></returns>
        private (int pageNo, int secNo, bool isFirst) GetPreviousPage(int BookId, int pageNo, int secNo, int pageNavigation)
        {
            if (pageNavigation == -1)
            {
                if (secNo == 1)
                {
                    var previousPageNumber = _db.Bundles
                        .Where(b => b.BookId == BookId && b.PageNo < pageNo)
                        .OrderByDescending(b => b.PageNo)
                        .Select(b => b.PageNo)
                        .FirstOrDefault();

                    if (previousPageNumber > 0)
                    {
                        var maxPreviousSecNo = _db.Bundles
                        .Where(b => b.BookId == BookId && b.PageNo == previousPageNumber)
                        .Select(b => b.SecNo)
                        .Max();

                        var isFirstPage = previousPageNumber == _db.Bundles.Where(b => b.BookId == BookId && b.PageNo < pageNo)
                            .Select(b => b.PageNo)
                            .Min();

                        var isfirstSection = maxPreviousSecNo == 1;

                        return (previousPageNumber, maxPreviousSecNo, isFirstPage && isfirstSection);
                    }
                    else
                    {
                        var firstpage = _db.Bundles.Where(b => b.BookId == BookId && b.PageNo == pageNo)
                            .Select(b => b.PageNo)
                            .FirstOrDefault();

                        var sectionMaxNumberInPreviousPage = _db.Bundles
                        .Where(b => b.BookId == BookId && b.PageNo == firstpage)
                        .Select(b => b.SecNo)
                        .Max();

                        return (firstpage, sectionMaxNumberInPreviousPage , true);
                    }
                }
                else if (secNo > 1)
                {
                    var previousSecNo = secNo - 1;
                    return (pageNo, previousSecNo, false);
                }
            }

            return (pageNo, secNo, false);
        }

        /// <summary>
        /// Get the Current Next QR Page and Section
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="pageNo"></param>
        /// <param name="secNo"></param>
        /// <param name="pageNavigation"></param>
        /// <returns></returns>
        private (int pageNo, int SecNo, bool isLastPageandSection) GetNextPage(int BookId, int pageNo, int secNo, int pageNavigation)
        {
            if (pageNavigation == 1)
            {
                var hasAnotherSection = _db.Bundles
                    .Any(b => b.BookId == BookId && b.PageNo == pageNo && b.SecNo > secNo);

                if (hasAnotherSection)
                {
                    var isLastPage = pageNo == _db.Bundles
                                     .Where(b => b.BookId == BookId)
                                     .Select(b => b.PageNo)
                                     .Max();

                    var isLastSection = secNo + 1 == _db.Bundles
                                    .Where(b => b.BookId == BookId && b.PageNo == pageNo)
                                    .Select(b => b.SecNo)
                                    .Max();

                    return (pageNo, secNo + 1, isLastPage && isLastSection);
                }
                else
                {
                    var nextPage = _db.Bundles
                     .Where(b => b.BookId == BookId && b.PageNo > pageNo)
                     .OrderBy(b => b.PageNo)
                     .Select(b => b.PageNo)
                     .FirstOrDefault();

                    var isLastPage = nextPage == _db.Bundles
                                     .Where(b => b.BookId == BookId)
                                     .Max(b => b.PageNo);

                    var isLastSection = secNo == _db.Bundles
                                    .Where(b => b.BookId == BookId && b.PageNo == nextPage)
                                    .Select(b => b.SecNo)

                                    .Max();
                    return (nextPage, 1, isLastPage && isLastSection);
                }

            }
            return (pageNo, secNo, false);
        }

        /// <summary>
        /// check if there is Page befoar Current Page
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="PageNo"></param>
        /// <param name="SecNo"></param>
        /// <returns></returns>
        private bool IstherePreviousPage(int? BookId, int? PageNo , int? SecNo)
        {
            if(SecNo == 1)
            {
                var previousPageNumber = _db.Bundles
                     .Where(b => b.BookId == BookId && b.PageNo < PageNo)
                     .OrderByDescending(b => b.PageNo)
                     .Select(b => b.PageNo)
                     .FirstOrDefault();

                if (previousPageNumber > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;         
        }

        /// <summary>
        /// check if there is Page after Current Page
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="PageNo"></param>
        /// <param name="SecNo"></param>
        /// <returns></returns>
        private bool IsThereNextPage(int? BookId, int? PageNo, int? SecNo)
        {
            var hasAnotherSection = _db.Bundles
                     .Any(b => b.BookId == BookId && b.PageNo == PageNo && b.SecNo > SecNo);

            if (hasAnotherSection)
            {
                var isLastPage = PageNo == _db.Bundles
                                 .Where(b => b.BookId == BookId)
                                 .Select(b => b.PageNo)
                                 .Max();

                var isLastSection = SecNo + 1 == _db.Bundles
                                .Where(b => b.BookId == BookId && b.PageNo == PageNo)
                                .Select(b => b.SecNo)
                                .Max();

                return (isLastPage && isLastSection);
            }
            else
            {
                var nextPage = _db.Bundles
                    .Where(b => b.BookId == BookId && b.PageNo > PageNo)
                    .OrderBy(b => b.PageNo)
                    .Select(b => b.PageNo)
                    .FirstOrDefault();

                if(nextPage == 0)
                {
                    return true;
                }
                else
                {
                    var isLastPage = nextPage == _db.Bundles
                               .Where(b => b.BookId == BookId)
                               .Max(b => b.PageNo);

                    var isLastSection = SecNo == _db.Bundles
                                    .Where(b => b.BookId == BookId && b.PageNo == nextPage)
                                    .Select(b => b.SecNo)
                                    .Max();

                    return isLastPage && isLastSection;
                }         
            }
        }

        public IEnumerable<BKRBundle> GetBundles(int bookId, int chapterNo)
        {
            var b = from bundle in _db.Bundles
                    join book in _db.Books on bundle.BookId equals book.Id
                    where book.Id == bookId
                    && bundle.ChapterNo == chapterNo
                    select new BKRBundle()
                    {
                        Book = new()
                        {
                            Name = book.Name,
                        },
                        AudioURL = book.URL + bundle.AudioURL,
                        ExternalURL = bundle.ExternalURL,
                        ExternalURLName = bundle.ExternalURLName,
                        ExternalVideo = bundle.ExternalVideo,
                        ImageURL = book.URL + bundle.ImageURL,
                        ExternalVideoName = bundle.ExternalVideoName,
                        InternalVideo = bundle.InternalVideo,
                        TextLanguage = bundle.TextLanguage.Trim(),
                        TextURL = book.URL + bundle.TextURL,
                        VideoURL = book.URL + bundle.VideoURL
                    };
            return b.ToList();
        }

        public IEnumerable<BKRBundle> GetBundles(int bookId)
        {
            var b = from bundle in _db.Bundles
                    join book in _db.Books on bundle.BookId equals book.Id
                    where book.Id == bookId
                    select new BKRBundle()
                    {
                        Book = new()
                        {
                            Name = book.Name,
                        },
                        AudioURL = book.URL + bundle.AudioURL,
                        ExternalURL = bundle.ExternalURL,
                        ExternalURLName = bundle.ExternalURLName,
                        ExternalVideo = bundle.ExternalVideo,
                        ImageURL = book.URL + bundle.ImageURL,
                        ExternalVideoName = bundle.ExternalVideoName,
                        InternalVideo = bundle.InternalVideo,
                        TextLanguage = bundle.TextLanguage.Trim(),
                        TextURL = book.URL + bundle.TextURL,
                        VideoURL = book.URL + bundle.VideoURL ?? null
                    };
            return b.ToList();
        }
    }
}
