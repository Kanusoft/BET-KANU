using BetKanu.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BetKanu.Data
{
    public class BKdbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductEpisode> ProductEpisodes { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public BKdbContext(DbContextOptions<BKdbContext> options) : base(options)
        {
        }

    }
}
