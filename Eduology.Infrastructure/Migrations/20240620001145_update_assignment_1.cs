using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eduology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_assignment_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "Files",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
