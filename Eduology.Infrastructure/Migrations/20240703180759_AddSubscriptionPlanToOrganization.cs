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

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_SubscriptionPlanId",
                table: "Organizations",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_SubscriptionPlans_SubscriptionPlanId",
                table: "Organizations",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "subscriptionPlanId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_SubscriptionPlans_SubscriptionPlanId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_SubscriptionPlanId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "Organizations");
        }
    }
}
