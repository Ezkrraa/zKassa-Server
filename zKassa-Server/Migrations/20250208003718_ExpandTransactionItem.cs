using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zKassa_Server.Migrations
{
    /// <inheritdoc />
    public partial class ExpandTransactionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PaidPrice",
                table: "TransactionItems",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TransactionItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidPrice",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TransactionItems");
        }
    }
}
