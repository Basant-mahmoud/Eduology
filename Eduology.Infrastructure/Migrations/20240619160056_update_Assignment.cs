using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eduology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_Assignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_AssignmentId",
                table: "Files");

            migrationBuilder.CreateIndex(
                name: "IX_Files_AssignmentId",
                table: "Files",
                column: "AssignmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_AssignmentId",
                table: "Files");

            migrationBuilder.CreateIndex(
                name: "IX_Files_AssignmentId",
                table: "Files",
                column: "AssignmentId");
        }
    }
}
