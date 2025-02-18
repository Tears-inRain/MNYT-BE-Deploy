using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Payment__DA638B192282CC45",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "Payment_id",
                table: "PaymentMethod",
                newName: "PaymentMethod_id");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_Account_id",
                table: "PaymentMethod",
                newName: "IX_PaymentMethod_Account_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__PaymentMethod__DA638B192282CC45",
                table: "PaymentMethod",
                column: "PaymentMethod_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__PaymentMethod__DA638B192282CC45",
                table: "PaymentMethod");

            migrationBuilder.RenameTable(
                name: "PaymentMethod",
                newName: "Payment");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod_id",
                table: "Payment",
                newName: "Payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentMethod_Account_id",
                table: "Payment",
                newName: "IX_Payment_Account_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Payment__DA638B192282CC45",
                table: "Payment",
                column: "Payment_id");
        }
    }
}
