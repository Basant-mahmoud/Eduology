using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eduology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatefile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId");
        }
    }
}
