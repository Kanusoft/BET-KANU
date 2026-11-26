using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetKanu.Data.Migrations
{
    /// <inheritdoc />
    public partial class GeneralizeCollaborationProjectContentAssociation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContentType",
                table: "CollaborationProjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "CollaborationProjects",
                type: "int",
                nullable: true);

            // Preserve existing BookId associations as ReaderBook (CollaborationContentType.ReaderBook = 2).
            migrationBuilder.Sql(
                @"UPDATE [CollaborationProjects]
SET
    [ContentType] = 2,
    [ContentId] = [BookId]
WHERE [BookId] IS NOT NULL;");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "CollaborationProjects");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationProjects_ContentType_ContentId",
                table: "CollaborationProjects",
                columns: new[] { "ContentType", "ContentId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_CollaborationProjects_ContentAssociation",
                table: "CollaborationProjects",
                sql: "([ContentType] IS NULL AND [ContentId] IS NULL) OR ([ContentType] IS NOT NULL AND [ContentId] IS NOT NULL AND [ContentId] > 0 AND [ContentType] IN (1, 2))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CollaborationProjects_ContentAssociation",
                table: "CollaborationProjects");

            migrationBuilder.DropIndex(
                name: "IX_CollaborationProjects_ContentType_ContentId",
                table: "CollaborationProjects");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "CollaborationProjects",
                type: "int",
                nullable: true);

            // Rollback limitation: only ReaderBook associations can be restored to BookId.
            // Product associations (ContentType = 1) cannot be represented in the old schema and are discarded.
            migrationBuilder.Sql(
                @"UPDATE [CollaborationProjects]
SET [BookId] = [ContentId]
WHERE [ContentType] = 2 AND [ContentId] IS NOT NULL;");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "CollaborationProjects");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "CollaborationProjects");
        }
    }
}
