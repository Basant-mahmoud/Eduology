using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eduology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionPlanToOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.SubscriptionPlanId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_SubscriptionPlanId",
                table: "Organizations",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_SubscriptionPlans_SubscriptionPlanId",
                table: "Organizations",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "SubscriptionPlanId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_SubscriptionPlans_SubscriptionPlanId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_SubscriptionPlanId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "Organizations");
        }
    }
}
