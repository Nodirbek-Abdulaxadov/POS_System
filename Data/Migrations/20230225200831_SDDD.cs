using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class SDDD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarningCount",
                table: "WarehousesItems");

            migrationBuilder.AddColumn<int>(
                name: "WarningCount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarningCount",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "WarningCount",
                table: "WarehousesItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
