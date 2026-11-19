using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetKanu.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerContributorCollaborations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Supporters_PublicListing",
                table: "Supporters");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Supporters_RecognitionYear",
                table: "Supporters");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Supporters_Type",
                table: "Supporters");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Supporters",
                newName: "SupporterType");

            migrationBuilder.RenameColumn(
                name: "RecognitionYear",
                table: "Supporters",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Supporters",
                newName: "IsVisible");

            migrationBuilder.RenameColumn(
                name: "DisplayLabel",
                table: "Supporters",
                newName: "Tag");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Supporters",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Supporters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "Supporters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "PartnerContributors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    PartnerType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerContributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollaborationProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerContributorId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProjectType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    ContentPageId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborationProjects_PartnerContributors_PartnerContributorId",
                        column: x => x.PartnerContributorId,
                        principalTable: "PartnerContributors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CollaborationMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerContributorId = table.Column<int>(type: "int", nullable: true),
                    CollaborationProjectId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationMedias", x => x.Id);
                    table.CheckConstraint("CK_CollaborationMedias_OneParent", "([PartnerContributorId] IS NOT NULL AND [CollaborationProjectId] IS NULL) OR ([PartnerContributorId] IS NULL AND [CollaborationProjectId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_CollaborationMedias_CollaborationProjects_CollaborationProjectId",
                        column: x => x.CollaborationProjectId,
                        principalTable: "CollaborationProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CollaborationMedias_PartnerContributors_PartnerContributorId",
                        column: x => x.PartnerContributorId,
                        principalTable: "PartnerContributors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationMedias_CollaborationProjectId_DisplayOrder",
                table: "CollaborationMedias",
                columns: new[] { "CollaborationProjectId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationMedias_PartnerContributorId_DisplayOrder",
                table: "CollaborationMedias",
                columns: new[] { "PartnerContributorId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationProjects_PartnerContributorId_DisplayOrder",
                table: "CollaborationProjects",
                columns: new[] { "PartnerContributorId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PartnerContributors_Category_IsActive_DisplayOrder",
                table: "PartnerContributors",
                columns: new[] { "Category", "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PartnerContributors_Year_IsActive",
                table: "PartnerContributors",
                columns: new[] { "Year", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaborationMedias");

            migrationBuilder.DropTable(
                name: "CollaborationProjects");

            migrationBuilder.DropTable(
                name: "PartnerContributors");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Supporters",
                newName: "RecognitionYear");

            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "Supporters",
                newName: "DisplayLabel");

            migrationBuilder.RenameColumn(
                name: "SupporterType",
                table: "Supporters",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "Supporters",
                newName: "IsPublished");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Supporters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RecognitionYear",
                table: "Supporters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Supporters",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_Supporters_PublicListing",
                table: "Supporters",
                columns: new[] { "IsPublished", "RecognitionYear", "Type", "DisplayOrder" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Supporters_RecognitionYear",
                table: "Supporters",
                sql: "[RecognitionYear] IS NULL OR ([RecognitionYear] BETWEEN 1900 AND 2100)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Supporters_Type",
                table: "Supporters",
                sql: "[Type] IN (1, 2, 3)");
        }
    }
}
