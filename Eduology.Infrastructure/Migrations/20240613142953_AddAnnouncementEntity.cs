using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Eduology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnouncementEntity : Migration
    {
        /// <inheritdoc />
protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_InstructorId",
                table: "Announcements",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_InstructorId",
                table: "Announcements",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // Change to Restrict
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_InstructorId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_InstructorId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Announcements");
        }
    }
}
