using BetKanu.Models;
using Microsoft.EntityFrameworkCore;


namespace BetKanu.Data
{
    public class BKdbContext : DbContext
    {
        public BKdbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductEpisode> ProductEpisodes { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
    }
}
