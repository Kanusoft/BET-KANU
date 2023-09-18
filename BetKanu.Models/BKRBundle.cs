using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models
{
    public class BKRBundle
    {
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

        public Book? Book { get; set; }

        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
    }
}
