using System;

namespace BetKanu.Models
{
    /// <summary>A content item and its position in the book.</summary>
    public sealed class BkReaderWindowItemDto
    {
        public int BundleId { get; set; }
        public int PageNo { get; set; }
        public int SecNo { get; set; }
        public int? ChapterNo { get; set; }
        public BKRBundle Content { get; set; } = default!;
    }

    /// <summary>The three-content reading window returned by BKReader V2.</summary>
    public sealed class BkReaderWindowDto
    {
        public int BookId { get; set; }
        public BkReaderWindowItemDto? Previous { get; set; }
        public BkReaderWindowItemDto Current { get; set; } = default!;
        public BkReaderWindowItemDto? Next { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }

    /// <summary>
    /// Raised when the QR code identifies multiple bundles with the same page
    /// and section (typically because a book restarts page numbering).
    /// </summary>
    public sealed class BkReaderAmbiguousPageException : Exception
    {
        public BkReaderAmbiguousPageException()
            : base("More than one bundle matches this page and section. Include chapterNo in the request.")
        {
        }
    }
}
