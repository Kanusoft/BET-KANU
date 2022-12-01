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

        public Bundle GetBundle(int id)
        {
            return _db.Bundles.FirstOrDefault(b => b.Id == id);
        }

        public BKRBundle GetBundle(int BookId, int pageNo, int SecNo)
        {
            var b = from bundle in _db.Bundles
                    join book in _db.Books on bundle.BookId equals book.Id
                    where book.Id == BookId
                    && bundle.PageNo == pageNo
                    && bundle.SecNo == SecNo
                    select new BKRBundle()
                    {
                        AudioURL = book.URL + bundle.AudioURL,
                        ExternalURL = bundle.ExternalURL,
                        ExternalURLName = bundle.ExternalURLName,
                        ExternalVideo = bundle.ExternalVideo,
                        ImageURL = book.URL + bundle.ImageURL,
                        ExternalVideoName = bundle.ExternalVideoName,
                        InternalVideo = bundle.InternalVideo,
                        TextLanguage = bundle.TextLanguage.Trim(),
                        TextURL = book.URL + bundle.TextURL,
                        VideoURL = bundle.VideoURL
                    };
            return b.FirstOrDefault();         
        }

        public IEnumerable<BKRBundle> GetBundles(int bookId, int chapterNo)
        {
            var b = from bundle in _db.Bundles
                    join book in _db.Books on bundle.BookId equals book.Id
                    where book.Id == bookId
                    && bundle.ChapterNo == chapterNo
                    select new BKRBundle()
                    {
                        AudioURL = book.URL + bundle.AudioURL,
                        ExternalURL = bundle.ExternalURL,
                        ExternalURLName = bundle.ExternalURLName,
                        ExternalVideo = bundle.ExternalVideo,
                        ImageURL = book.URL + bundle.ImageURL,
                        ExternalVideoName = bundle.ExternalVideoName,
                        InternalVideo = bundle.InternalVideo,
                        TextLanguage = bundle.TextLanguage.Trim(),
                        TextURL = book.URL + bundle.TextURL,
                        VideoURL = bundle.VideoURL
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
                        AudioURL = book.URL + bundle.AudioURL,
                        ExternalURL = bundle.ExternalURL,
                        ExternalURLName = bundle.ExternalURLName,
                        ExternalVideo = bundle.ExternalVideo,
                        ImageURL = book.URL + bundle.ImageURL,
                        ExternalVideoName = bundle.ExternalVideoName,
                        InternalVideo = bundle.InternalVideo,
                        TextLanguage = bundle.TextLanguage.Trim(),
                        TextURL = book.URL + bundle.TextURL,
                        VideoURL = bundle.VideoURL
                    };
            return b.ToList();
        }
    }
}
