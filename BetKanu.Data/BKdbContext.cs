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

        public DbSet<Magazine> Magazines { get; set; } = null!;
        public DbSet<MagazineArticle> MagazineArticles { get; set; } = null!;

        public BKdbContext(DbContextOptions<BKdbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Magazine>(entity =>
            {
                entity.HasIndex(x => x.Slug)
                    .IsUnique();

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Slug)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.LongDescription)
                    .HasColumnType("nvarchar(max)");

                entity.HasMany(x => x.Articles)
                    .WithOne(x => x.Magazine)
                    .HasForeignKey(x => x.MagazineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MagazineArticle>(entity =>
            {
                entity.HasIndex(x => new
                {
                    x.MagazineId,
                    x.Slug
                })
                .IsUnique();

                entity.HasIndex(x => new
                {
                    x.MagazineId,
                    x.ArticleNumber
                })
                .IsUnique();

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.Slug)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(x => x.WesternIntroduction)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.WesternBodyHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.WesternCreditsHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EasternIntroduction)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EasternBodyHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EasternCreditsHtml)
                    .HasColumnType("nvarchar(max)");
            });
        }
    }
}
