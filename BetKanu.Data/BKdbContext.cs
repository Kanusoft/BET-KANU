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
        public DbSet<Supporter> Supporters { get; set; } = null!;
        public DbSet<PartnerContributor> PartnerContributors => Set<PartnerContributor>();
        public DbSet<CollaborationProject> CollaborationProjects => Set<CollaborationProject>();
        public DbSet<CollaborationMedia> CollaborationMedias => Set<CollaborationMedia>();

        public BKdbContext(DbContextOptions<BKdbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(
        ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Supporter>(entity =>
            {
                entity.ToTable("Supporters");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Type)
                    .HasColumnName("SupporterType")
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(x => x.Period)
                    .HasColumnName("Period")
                    .IsRequired();

                entity.Property(x => x.RecognitionYear)
                    .HasColumnName("Year")
                    .IsRequired();

                entity.Property(x => x.DisplayLabel)
                    .HasColumnName("Tag")
                    .HasMaxLength(120);

                entity.Property(x => x.IsPublished)
                    .HasColumnName("IsVisible")
                    .IsRequired();

                entity.Property(x => x.DisplayOrder)
                    .IsRequired();

                entity.Property(x => x.CreatedAtUtc)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(x => x.UpdatedAtUtc)
                    .HasColumnType("datetime2");
            });

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

                entity.Property(x => x.ShortDescription)
                    .HasMaxLength(500);

                entity.Property(x => x.LongDescription)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.CoverImage)
                    .HasMaxLength(500);

                entity.Property(x => x.CoverImage350)
                    .HasMaxLength(500);

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

                entity.Property(x => x.ShortDescription)
                    .HasMaxLength(1000);

                entity.Property(x => x.BannerImage)
                    .HasMaxLength(500);

                entity.Property(x => x.BannerImage350)
                    .HasMaxLength(500);

                entity.Property(x => x.WesternTitle)
                    .HasMaxLength(250);

                entity.Property(x => x.WesternIntroduction)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.WesternBodyHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.WesternCreditsHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.WesternPdfPath)
                    .HasMaxLength(500);

                entity.Property(x => x.EasternTitle)
                    .HasMaxLength(250);

                entity.Property(x => x.EasternIntroduction)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EasternBodyHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EasternCreditsHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EasternPdfPath)
                    .HasMaxLength(500);

                entity.Property(x => x.EnglishTitle)
    .HasMaxLength(250);

                entity.Property(x => x.EnglishIntroduction)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EnglishBodyHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EnglishCreditsHtml)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.EnglishPdfPath)
                    .HasMaxLength(500);
            });

            builder.Entity<PartnerContributor>(entity =>
            {
                entity.ToTable("PartnerContributors");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.LogoUrl).HasMaxLength(1000);
                entity.Property(x => x.ShortDescription).HasMaxLength(1000);
                entity.Property(x => x.LongDescription)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.WebsiteUrl).HasMaxLength(1000);
                entity.Property(x => x.PartnerType).HasMaxLength(150);

                entity.Property(x => x.Category)
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2");

                entity.Property(x => x.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.HasIndex(x => new
                {
                    x.Category,
                    x.IsActive,
                    x.DisplayOrder
                });

                entity.HasIndex(x => new
                {
                    x.Year,
                    x.IsActive
                });

                entity.HasMany(x => x.Projects)
                    .WithOne(x => x.PartnerContributor)
                    .HasForeignKey(x => x.PartnerContributorId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<CollaborationProject>(entity =>
            {
                entity.ToTable("CollaborationProjects", table =>
                    table.HasCheckConstraint(
                        "CK_CollaborationProjects_ContentAssociation",
                        "([ContentType] IS NULL AND [ContentId] IS NULL) " +
                        "OR ([ContentType] IS NOT NULL AND [ContentId] IS NOT NULL " +
                        "AND [ContentId] > 0 AND [ContentType] IN (1, 2))"));

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.LogoUrl).HasMaxLength(1000);
                entity.Property(x => x.ShortDescription).HasMaxLength(1000);
                entity.Property(x => x.LongDescription)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.ProjectLink).HasMaxLength(1000);
                entity.Property(x => x.ProjectType).HasMaxLength(150);

                entity.Property(x => x.ContentType)
                    .HasConversion<int?>();

                entity.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2");

                entity.Property(x => x.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.HasIndex(x => new
                {
                    x.PartnerContributorId,
                    x.DisplayOrder
                });

                entity.HasIndex(x => new
                {
                    x.ContentType,
                    x.ContentId
                });
            });

            builder.Entity<CollaborationMedia>(entity =>
            {
                entity.ToTable("CollaborationMedias", table =>
                    table.HasCheckConstraint(
                        "CK_CollaborationMedias_OneParent",
                        "([PartnerContributorId] IS NOT NULL " +
                        "AND [CollaborationProjectId] IS NULL) " +
                        "OR ([PartnerContributorId] IS NULL " +
                        "AND [CollaborationProjectId] IS NOT NULL)"));

                entity.HasKey(x => x.Id);

                entity.Property(x => x.FileName)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(x => x.FileUrl)
                    .IsRequired()
                    .HasMaxLength(1500);

                entity.Property(x => x.BlobName).HasMaxLength(700);
                entity.Property(x => x.ContentType).HasMaxLength(150);

                entity.Property(x => x.MediaType)
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(x => x.Title).HasMaxLength(250);
                entity.Property(x => x.Caption).HasMaxLength(1000);
                entity.Property(x => x.Description)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.ThumbnailUrl)
                    .HasMaxLength(1500);

                entity.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2");

                entity.HasOne(x => x.PartnerContributor)
                    .WithMany(x => x.Media)
                    .HasForeignKey(x => x.PartnerContributorId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.CollaborationProject)
                    .WithMany(x => x.Media)
                    .HasForeignKey(x => x.CollaborationProjectId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(x => new
                {
                    x.PartnerContributorId,
                    x.DisplayOrder
                });

                entity.HasIndex(x => new
                {
                    x.CollaborationProjectId,
                    x.DisplayOrder
                });
            });
        }
    }
}
