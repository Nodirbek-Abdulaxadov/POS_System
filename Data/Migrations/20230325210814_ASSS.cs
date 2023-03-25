using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ASSS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Transactions",
                newName: "ReceiptId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Loans",
                newName: "ReceiptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiptId",
                table: "Transactions",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "ReceiptId",
                table: "Loans",
                newName: "OrderId");
        }
    }
}
