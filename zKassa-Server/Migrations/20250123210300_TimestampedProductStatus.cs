using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zKassa_Server.Migrations
{
    /// <inheritdoc />
    public partial class TimestampedProductStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "ProductStatuses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses",
                columns: new[] { "ProductId", "DistributionCenterId", "TimeStamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "ProductStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses",
                columns: new[] { "ProductId", "DistributionCenterId" });
        }
    }
}
