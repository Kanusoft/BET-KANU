using BetKanu.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BetKanu.Data
{
    public class BKdbContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductEpisode> ProductEpisodes { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bundle> Bundles { get; set; }

        public BKdbContext(DbContextOptions<BKdbContext> options) : base(options)
        {
        }

    }
}
