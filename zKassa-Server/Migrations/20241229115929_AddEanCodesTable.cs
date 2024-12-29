using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zKassa_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEanCodesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EanCode_Products_ProductId",
                table: "EanCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EanCode",
                table: "EanCode");

            migrationBuilder.RenameTable(
                name: "EanCode",
                newName: "EanCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EanCodes",
                table: "EanCodes",
                columns: new[] { "ProductId", "EAN" });

            migrationBuilder.AddForeignKey(
                name: "FK_EanCodes_Products_ProductId",
                table: "EanCodes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EanCodes_Products_ProductId",
                table: "EanCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EanCodes",
                table: "EanCodes");

            migrationBuilder.RenameTable(
                name: "EanCodes",
                newName: "EanCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EanCode",
                table: "EanCode",
                columns: new[] { "ProductId", "EAN" });

            migrationBuilder.AddForeignKey(
                name: "FK_EanCode_Products_ProductId",
                table: "EanCode",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
