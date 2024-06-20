using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eduology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Organization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "ConfirmPassword",
            table: "Organizations",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "ConfirmPassword",
            table: "Organizations");
        }
    }
}
