using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zKassa_Server.Migrations
{
    /// <inheritdoc />
    public partial class MoreProductFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PlasticTax",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalesTax",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PlasticTax",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SalesTax",
                table: "Products");
        }
    }
}
