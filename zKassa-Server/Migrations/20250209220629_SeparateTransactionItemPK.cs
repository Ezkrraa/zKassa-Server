using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zKassa_Server.Migrations
{
    /// <inheritdoc />
    public partial class SeparateTransactionItemPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TransactionItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_TransactionId",
                table: "TransactionItems",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItems_TransactionId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TransactionItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems",
                columns: new[] { "TransactionId", "ProductId" });
        }
    }
}
