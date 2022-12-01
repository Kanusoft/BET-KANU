using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models
{
    public class Bundle
    {
        [Key]
        public int Id { get; set; }
        public int PageNo { get; set; }
        public int SecNo { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string? XMLUrlId { get; set; }
        public int ChapterNo { get; set; }
        public string? ImageURL { get; set; }
        public string? TextURL { get; set; }
        public string? TextLanguage { get; set; }
        public string? AudioURL { get; set; }
        public string? VideoURL { get; set; }
        public string? ExternalVideo { get; set; }
        public string? InternalVideo { get; set; }
        public string? ExternalURL { get; set; }
        public string? ExternalURLName { get; set; }
        public string? ExternalVideoName { get; set; }
    }
}
