using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetKanu.Data.Migrations
{
    /// <inheritdoc />
    public partial class addMagazineTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "source",
                table: "Products",
                newName: "Source");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Shops",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreditsM",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishScript",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRelease",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MalouliScript",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mpdf",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SponsoredBY",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoM",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewsM",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Magazines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MagazineArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MagazineId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ArticleNumber = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BannerImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WesternTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WesternIntroduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WesternBodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WesternCreditsHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WesternPdfPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EasternTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EasternIntroduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EasternBodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EasternCreditsHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EasternPdfPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PublishAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagazineArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MagazineArticles_Magazines_MagazineId",
                        column: x => x.MagazineId,
                        principalTable: "Magazines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MagazineArticles_MagazineId_ArticleNumber",
                table: "MagazineArticles",
                columns: new[] { "MagazineId", "ArticleNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MagazineArticles_MagazineId_Slug",
                table: "MagazineArticles",
                columns: new[] { "MagazineId", "Slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Magazines_Slug",
                table: "Magazines",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MagazineArticles");

            migrationBuilder.DropTable(
                name: "Magazines");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "CreditsM",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EnglishScript",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsRelease",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MalouliScript",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Mpdf",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SponsoredBY",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VideoM",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ViewsM",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "Products",
                newName: "source");
        }
    }
}
