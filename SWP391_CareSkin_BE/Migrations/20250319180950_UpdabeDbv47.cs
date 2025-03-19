using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdabeDbv47 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZaloPayOrders_Order_OrderId",
                table: "ZaloPayOrders");

            migrationBuilder.DropIndex(
                name: "IX_ZaloPayOrders_OrderId",
                table: "ZaloPayOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ZaloPayOrders");

            migrationBuilder.AddColumn<string>(
                name: "Items",
                table: "ZaloPayOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ZaloPayCallbacks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Items",
                table: "ZaloPayOrders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ZaloPayOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Amount",
                table: "ZaloPayCallbacks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_ZaloPayOrders_OrderId",
                table: "ZaloPayOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZaloPayOrders_Order_OrderId",
                table: "ZaloPayOrders",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
