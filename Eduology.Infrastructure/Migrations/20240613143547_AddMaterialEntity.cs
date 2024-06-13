using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Eduology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "Materials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_InstructorId",
                table: "Materials",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_AspNetUsers_InstructorId",
                table: "Materials",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
   
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_AspNetUsers_InstructorId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_InstructorId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Materials");
        }
    }
}
