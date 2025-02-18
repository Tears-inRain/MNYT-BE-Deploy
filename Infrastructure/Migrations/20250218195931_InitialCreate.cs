using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Payment__Account__403A8C7D",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_Account_id",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Transaction_code",
                table: "Account_membership");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Account_membership",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Account_id",
                table: "Payment",
                column: "Account_id",
                unique: true,
                filter: "[Account_id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK__Payment__Account__403A8C7D",
                table: "Payment",
                column: "Account_id",
                principalTable: "Account_membership",
                principalColumn: "membership_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Payment__Account__403A8C7D",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_Account_id",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Account_membership");

            migrationBuilder.AddColumn<string>(
                name: "Transaction_code",
                table: "Account_membership",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Account_id",
                table: "Payment",
                column: "Account_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Payment__Account__403A8C7D",
                table: "Payment",
                column: "Account_id",
                principalTable: "Account",
                principalColumn: "Account_id");
        }
    }
}
