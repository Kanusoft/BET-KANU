using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetKanu.Data.Migrations
{
    /// <inheritdoc />
    public partial class firstmigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dialect = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    IsDownloadable = table.Column<bool>(type: "bit", nullable: false),
                    DownloadableDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetAudince = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScriptE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScriptW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditsE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditsW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wpdf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Epdf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewsE = table.Column<int>(type: "int", nullable: false),
                    ViewsW = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bundles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageNo = table.Column<int>(type: "int", nullable: false),
                    SecNo = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    XMLUrlId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterNo = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalURLName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalVideoName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bundles_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductEpisodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Views = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEpisodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductEpisodes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bundles_BookId",
                table: "Bundles",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEpisodes_ProductId",
                table: "ProductEpisodes",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bundles");

            migrationBuilder.DropTable(
                name: "ProductEpisodes");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
