using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.EntityFrameworkCore;

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

        public BKRBundle? GetBundle(int? BookId, int? pageNo, int? SecNo, int? pageNavigation)
        {
            bool isFirstPageAndSection = false;
            bool isLastPageAndSection = false;
            //Get Previous QR Bundle 
            if (pageNavigation is -1)
            {

                (int newPageNo, int newSecNo) = GetPreviousPage(BookId ?? 0, pageNo ?? 0, SecNo ?? 0, pageNavigation ?? 0);

                isFirstPageAndSection = IsTherePreviousPage(BookId, newPageNo, newSecNo);
                isLastPageAndSection = IsThereNextPage(BookId, newPageNo, newSecNo);

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
                            AudioURL = (book.URL + bundle.AudioURL).Replace("\r", "").Replace("\n", ""),
                            ExternalURL = bundle.ExternalURL,
                            ExternalURLName = bundle.ExternalURLName,
                            ExternalVideo = bundle.ExternalVideo,
                            ImageURL = (book.URL + bundle.ImageURL).Replace("\r", "").Replace("\n", ""),
                            ExternalVideoName = bundle.ExternalVideoName,
                            InternalVideo = bundle.InternalVideo,
                            TextLanguage = bundle.TextLanguage.Trim(),
                            TextURL = (book.URL + bundle.TextURL).Replace("\r", "").Replace("\n", ""),
                            VideoURL = book.URL + bundle.VideoURL,
                            IsFirst = isFirstPageAndSection,
                            IsLast = isLastPageAndSection,
                            NewPageNo = newPageNo,
                            NewSecNo = newSecNo,
                        };
                return b.FirstOrDefault();

            }
            //Get the Current QR Page
            else if (pageNavigation is 0)
            {
                isFirstPageAndSection = IsTherePreviousPage(BookId, pageNo, SecNo);
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
                            AudioURL = (book.URL + bundle.AudioURL).Replace("\r", "").Replace("\n", ""),
                            ExternalURL = bundle.ExternalURL,
                            ExternalURLName = bundle.ExternalURLName,
                            ExternalVideo = bundle.ExternalVideo,
                            ImageURL = (book.URL + bundle.ImageURL).Replace("\r", "").Replace("\n", ""),
                            ExternalVideoName = bundle.ExternalVideoName,
                            InternalVideo = bundle.InternalVideo,
                            TextLanguage = bundle.TextLanguage.Trim(),
                            TextURL = (book.URL + bundle.TextURL).Replace("\r", "").Replace("\n", ""),
                            VideoURL = book.URL + bundle.VideoURL,
                            IsFirst = isFirstPageAndSection,
                            IsLast = isLastPageAndSection,

                        };

                return b.FirstOrDefault();

            }
            //Get The Next QR Page
            else if (pageNavigation is 1)
            {

                (int newPageNo, int newSecNo) = GetNextPage(BookId ?? 0, pageNo ?? 0, SecNo ?? 0, pageNavigation ?? 0);

                isFirstPageAndSection = IsTherePreviousPage(BookId, newPageNo, newSecNo);
                isLastPageAndSection = IsThereNextPage(BookId, newPageNo, newSecNo);


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
                            AudioURL = (book.URL + bundle.AudioURL).Replace("\r", "").Replace("\n", ""),
                            ExternalURL = bundle.ExternalURL,
                            ExternalURLName = bundle.ExternalURLName,
                            ExternalVideo = bundle.ExternalVideo,
                            ImageURL = (book.URL + bundle.ImageURL).Replace("\r", "").Replace("\n", ""),
                            ExternalVideoName = bundle.ExternalVideoName,
                            InternalVideo = bundle.InternalVideo,
                            TextLanguage = bundle.TextLanguage.Trim(),
                            TextURL = (book.URL + bundle.TextURL).Replace("\r", "").Replace("\n", ""),
                            VideoURL = book.URL + bundle.VideoURL,
                            IsFirst = isFirstPageAndSection,
                            IsLast = isLastPageAndSection,
                            NewPageNo = newPageNo,
                            NewSecNo = newSecNo,
                        };
                return b.FirstOrDefault();
            }
            return null;
        }

        //public BKRBundle GetBundle(int? bookId, int? pageNo, int? secNo=1 , int? pageNavigation = 0)
        //{
        //    var b = from bundle in _db.Bundles
        //            join book in _db.Books on bundle.BookId equals book.Id
        //            where book.Id == bookId
        //            && bundle.PageNo == pageNo
        //            && bundle.SecNo == secNo
        //            select new BKRBundle()
        //            {                     
        //                AudioURL = (book.URL + bundle.AudioURL).Replace("\r", "").Replace("\n", ""),
        //                ExternalURL = bundle.ExternalURL,
        //                ExternalURLName = bundle.ExternalURLName,
        //                ExternalVideo = bundle.ExternalVideo,
        //                ImageURL = (book.URL + bundle.ImageURL).Replace("\r", "").Replace("\n", ""),
        //                ExternalVideoName = bundle.ExternalVideoName,
        //                InternalVideo = bundle.InternalVideo,
        //                TextLanguage = bundle.TextLanguage.Trim(),
        //                TextURL = (book.URL + bundle.TextURL).Replace("\r", "").Replace("\n", ""),
        //                VideoURL = book.URL + bundle.VideoURL
        //            };
        //    return b.FirstOrDefault();
        //}

        /// <summary>
        /// Get the Current Previous QR Page and Section
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="pageNo"></param>
        /// <param name="secNo"></param>
        /// <param name="pageNavigation"></param>
        /// <returns></returns>
        private (int pageNo, int secNo) GetPreviousPage(int BookId, int pageNo, int secNo, int pageNavigation)
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

                        return (previousPageNumber, maxPreviousSecNo);
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

                        return (firstpage, sectionMaxNumberInPreviousPage);
                    }
                }
                else if (secNo > 1)
                {
                    var previousSecNo = secNo - 1;
                    return (pageNo, previousSecNo);
                }
            }

            return (pageNo, secNo);
        }

        /// <summary>
        /// Get the Current Next QR Page and Section
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="pageNo"></param>
        /// <param name="secNo"></param>
        /// <param name="pageNavigation"></param>
        /// <returns></returns>
        private (int pageNo, int SecNo) GetNextPage(int BookId, int pageNo, int secNo, int pageNavigation)
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

                    return (pageNo, secNo + 1);
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
                    return (nextPage, 1);
                }

            }
            return (pageNo, secNo);
        }

        /// <summary>
        /// check if there is Page befoar Current Page
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="PageNo"></param>
        /// <param name="SecNo"></param>
        /// <returns></returns>
        private bool IsTherePreviousPage(int? BookId, int? PageNo , int? SecNo)
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
                        //Book = new()
                        //{
                        //    Name = book.Name,
                        //},
                        AudioURL = (book.URL + bundle.AudioURL).Replace("\r", "").Replace("\n", ""),
                        ExternalURL = bundle.ExternalURL,
                        ExternalURLName = bundle.ExternalURLName,
                        ExternalVideo = bundle.ExternalVideo,
                        ImageURL = (book.URL + bundle.ImageURL).Replace("\r", "").Replace("\n", ""),
                        ExternalVideoName = bundle.ExternalVideoName,
                        InternalVideo = bundle.InternalVideo,
                        TextLanguage = bundle.TextLanguage.Trim(),
                        TextURL = (book.URL + bundle.TextURL).Replace("\r", "").Replace("\n", ""),
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
                        //Book = new()
                        //{
                        //    Name = book.Name,
                        //},
                        AudioURL = (book.URL + bundle.AudioURL).Replace("\r", "").Replace("\n", ""),
                        ExternalURL = bundle.ExternalURL,
                        ExternalURLName = bundle.ExternalURLName,
                        ExternalVideo = bundle.ExternalVideo,
                        ImageURL = (book.URL + bundle.ImageURL).Replace("\r", "").Replace("\n", ""),
                        ExternalVideoName = bundle.ExternalVideoName,
                        InternalVideo = bundle.InternalVideo,
                        TextLanguage = bundle.TextLanguage.Trim(),
                        TextURL = (book.URL + bundle.TextURL).Replace("\r", "").Replace("\n", ""),
                        VideoURL = book.URL + bundle.VideoURL ?? null
                    };
            return b.ToList();
        }

        /// <summary>
        /// Fetches the scanned bundle plus its immediate previous and next bundles.
        /// Default reading order: PageNo -> SecNo, regardless of ChapterNo.
        /// If chapterNo is explicitly supplied, use chapter-aware ordering for
        /// exceptional books whose page numbering restarts in each chapter.
        /// </summary>
        public async Task<BkReaderWindowDto?> GetReaderWindowAsync(
            int bookId,
            int pageNo,
            int secNo,
            int? chapterNo = null,
            CancellationToken cancellationToken = default)
        {
            // Do not expose unreleased content from the public V2 endpoint.
            var book = await _db.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == bookId && b.IsReleased == true,
                    cancellationToken);

            if (book is null)
                return null;

            var bundles = _db.Bundles
                .AsNoTracking()
                .Where(b => b.BookId == bookId);

            var currentQuery = bundles.Where(b =>
                b.PageNo == pageNo && b.SecNo == secNo);

            // Normally unused. If an exceptional book restarts page numbers
            // in later chapters, clients can pass chapterNo explicitly.
            if (chapterNo.HasValue)
                currentQuery = currentQuery.Where(b => b.ChapterNo == chapterNo.Value);

            var matches = await currentQuery
                .OrderBy(b => b.Id)
                .Take(2)
                .ToListAsync(cancellationToken);

            if (matches.Count == 0)
                return null;

            // If the QR can identify more than one bundle, never guess.
            if (matches.Count > 1)
                throw new BkReaderAmbiguousPageException();

            var current = matches[0];

            // Return two candidates on either side so IsFirst/IsLast on
            // each of the three returned content items can be set correctly.
            IQueryable<Bundle> previousQuery;
            IQueryable<Bundle> nextQuery;
            IOrderedQueryable<Bundle> previousOrdered;
            IOrderedQueryable<Bundle> nextOrdered;

            if (chapterNo.HasValue)
            {
                // Opt-in compatibility mode for chapter-based numbering.
                int currentChapter = (int?)current.ChapterNo ?? 0;
                previousQuery = bundles.Where(b =>
                    ((int?)b.ChapterNo ?? 0) < currentChapter ||
                    (((int?)b.ChapterNo ?? 0) == currentChapter &&
                     (b.PageNo < current.PageNo ||
                      (b.PageNo == current.PageNo && b.SecNo < current.SecNo))));

                nextQuery = bundles.Where(b =>
                    ((int?)b.ChapterNo ?? 0) > currentChapter ||
                    (((int?)b.ChapterNo ?? 0) == currentChapter &&
                     (b.PageNo > current.PageNo ||
                      (b.PageNo == current.PageNo && b.SecNo > current.SecNo))));

                previousOrdered = previousQuery
                    .OrderByDescending(b => (int?)b.ChapterNo ?? 0)
                    .ThenByDescending(b => b.PageNo)
                    .ThenByDescending(b => b.SecNo)
                    .ThenByDescending(b => b.Id);

                nextOrdered = nextQuery
                    .OrderBy(b => (int?)b.ChapterNo ?? 0)
                    .ThenBy(b => b.PageNo)
                    .ThenBy(b => b.SecNo)
                    .ThenBy(b => b.Id);
            }
            else
            {
                // DEFAULT: Ignore ChapterNo completely while navigating.
                // This handles skipped page/section numbers correctly.
                previousQuery = bundles.Where(b =>
                    b.PageNo < current.PageNo ||
                    (b.PageNo == current.PageNo && b.SecNo < current.SecNo));

                nextQuery = bundles.Where(b =>
                    b.PageNo > current.PageNo ||
                    (b.PageNo == current.PageNo && b.SecNo > current.SecNo));

                previousOrdered = previousQuery
                    .OrderByDescending(b => b.PageNo)
                    .ThenByDescending(b => b.SecNo)
                    .ThenByDescending(b => b.Id);

                nextOrdered = nextQuery
                    .OrderBy(b => b.PageNo)
                    .ThenBy(b => b.SecNo)
                    .ThenBy(b => b.Id);
            }

            var previousCandidates = await previousOrdered
                .Take(2)
                .ToListAsync(cancellationToken);

            var nextCandidates = await nextOrdered
                .Take(2)
                .ToListAsync(cancellationToken);

            var currentItem = MapReaderItem(current, book.Name,book.DownloadableDocument ?? string.Empty, book.URL);
            currentItem.Content.IsFirst = previousCandidates.Count == 0;
            currentItem.Content.IsLast = nextCandidates.Count == 0;

            BkReaderWindowItemDto? previousItem = null;
            if (previousCandidates.Count > 0)
            {
                previousItem = MapReaderItem(previousCandidates[0], book.Name, book.DownloadableDocument ?? string.Empty, book.URL);
                previousItem.Content.IsFirst = previousCandidates.Count == 1;
                previousItem.Content.IsLast = false;
            }

            BkReaderWindowItemDto? nextItem = null;
            if (nextCandidates.Count > 0)
            {
                nextItem = MapReaderItem(nextCandidates[0], book.Name, book.DownloadableDocument ?? string.Empty, book.URL);
                nextItem.Content.IsFirst = false;
                nextItem.Content.IsLast = nextCandidates.Count == 1;
            }

            return new BkReaderWindowDto
            {
                BookId = bookId,
                Previous = previousItem,
                Current = currentItem,
                Next = nextItem,
                HasPrevious = previousItem != null,
                HasNext = nextItem != null
            };
        }

        private static BkReaderWindowItemDto MapReaderItem(
            Bundle bundle, string? bookName,string bookDownloadURL, string? bookUrl)
        {
            return new BkReaderWindowItemDto
            {
                BundleId = bundle.Id,
                PageNo = bundle.PageNo,
                SecNo = bundle.SecNo,
                ChapterNo = bundle.ChapterNo,
                Content = new BKRBundle
                {
                    Book = new()
                    {
                        Name = bookName,
                        DownloadableDocument = bookDownloadURL,
                    },
                    AudioURL = (bookUrl + bundle.AudioURL).Replace("\r", "").Replace("\n", ""),
                    ExternalURL = bundle.ExternalURL,
                    ExternalURLName = bundle.ExternalURLName,
                    ExternalVideo = bundle.ExternalVideo,
                    ImageURL = (bookUrl + bundle.ImageURL).Replace("\r", "").Replace("\n", ""),
                    ExternalVideoName = bundle.ExternalVideoName,
                    InternalVideo = bundle.InternalVideo,
                    TextLanguage = bundle.TextLanguage?.Trim() ?? string.Empty,
                    TextURL = (bookUrl + bundle.TextURL).Replace("\r", "").Replace("\n", ""),
                    VideoURL = bookUrl + bundle.VideoURL,
                    NewPageNo = bundle.PageNo,
                    NewSecNo = bundle.SecNo
                }
            };
        }
    }
}
