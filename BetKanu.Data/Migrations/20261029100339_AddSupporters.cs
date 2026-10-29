using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetKanu.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSupporters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ChapterNo",
                table: "Bundles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Supporters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RecognitionYear = table.Column<int>(type: "int", nullable: true),
                    DisplayLabel = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supporters", x => x.Id);
                    table.CheckConstraint("CK_Supporters_RecognitionYear", "[RecognitionYear] IS NULL OR ([RecognitionYear] BETWEEN 1900 AND 2100)");
                    table.CheckConstraint("CK_Supporters_Type", "[Type] IN (1, 2, 3)");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Supporters_PublicListing",
                table: "Supporters",
                columns: new[] { "IsPublished", "RecognitionYear", "Type", "DisplayOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Supporters");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterNo",
                table: "Bundles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
