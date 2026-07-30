using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetKanu.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnglishLyricsMagazineArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnglishBodyHtml",
                table: "MagazineArticles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishCreditsHtml",
                table: "MagazineArticles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishIntroduction",
                table: "MagazineArticles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishPdfPath",
                table: "MagazineArticles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishTitle",
                table: "MagazineArticles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishBodyHtml",
                table: "MagazineArticles");

            migrationBuilder.DropColumn(
                name: "EnglishCreditsHtml",
                table: "MagazineArticles");

            migrationBuilder.DropColumn(
                name: "EnglishIntroduction",
                table: "MagazineArticles");

            migrationBuilder.DropColumn(
                name: "EnglishPdfPath",
                table: "MagazineArticles");

            migrationBuilder.DropColumn(
                name: "EnglishTitle",
                table: "MagazineArticles");
        }
    }
}
