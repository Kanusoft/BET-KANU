using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models.Interface
{
    public interface IBKBundle
    {
        Book GetBook(int id);
        IEnumerable<Book> GetBooks();
        Bundle? GetBundle(int? id);
        BKRBundle? GetBundle(int? BookId, int? pageNo, int? SecNo);
        IEnumerable<BKRBundle> GetBundles(int bookId, int chapterNo);
        IEnumerable<BKRBundle> GetBundles(int bookId);
    }
}
