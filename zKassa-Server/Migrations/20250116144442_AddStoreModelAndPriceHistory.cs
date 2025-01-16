using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zKassa_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreModelAndPriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "AmountInBox",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DistributionCenters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceLogs",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLogs", x => new { x.ProductId, x.TimeStamp });
                    table.ForeignKey(
                        name: "FK_PriceLogs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStatuses",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DistributionCenterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatuses", x => new { x.ProductId, x.DistributionCenterId });
                    table.ForeignKey(
                        name: "FK_ProductStatuses_DistributionCenters_DistributionCenterId",
                        column: x => x.DistributionCenterId,
                        principalTable: "DistributionCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStatuses_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DistributionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_DistributionCenters_DistributionId",
                        column: x => x.DistributionId,
                        principalTable: "DistributionCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShopId",
                table: "AspNetUsers",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatuses_DistributionCenterId",
                table: "ProductStatuses",
                column: "DistributionCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_DistributionId",
                table: "Shops",
                column: "DistributionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Shops_ShopId",
                table: "AspNetUsers",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Shops_ShopId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PriceLogs");

            migrationBuilder.DropTable(
                name: "ProductStatuses");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "DistributionCenters");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShopId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AmountInBox",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "AspNetUsers");
        }
    }
}
