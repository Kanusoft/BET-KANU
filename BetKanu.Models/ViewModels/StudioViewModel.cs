using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models.ViewModels
{
    public class StudioViewModel
    {
        public List<Product>? Products { get; set; }
        public Product? product { get; set; }

        public List<ProductEpisode>? Episodes { get; set; }

        public List<ProductEpisode>? WestrenEpisodes { get; set; }
        public List<ProductEpisode>? EastrenEpisodes { get; set; }
    }
}
