using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? URL { get; set; }
        public string? Dialect { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public bool IsReleased { get; set; }
        public bool IsDownloadable { get; set; }
        public string? DownloadableDocument { get; set; }
        public string? ProjectURL { get; set; }
        public string? BookImageURL { get; set; }
    }
}
