using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Promotion_PromotionId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Promotion",
                newName: "PromotionName");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariationId",
                table: "OrderProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariationId",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DoB",
                table: "Admin",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductVariationId",
                table: "OrderProduct",
                column: "ProductVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductVariationId",
                table: "Cart",
                column: "ProductVariationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_ProductVariation_ProductVariationId",
                table: "Cart",
                column: "ProductVariationId",
                principalTable: "ProductVariation",
                principalColumn: "ProductVariationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Promotion_PromotionId",
                table: "Order",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_ProductVariation_ProductVariationId",
                table: "OrderProduct",
                column: "ProductVariationId",
                principalTable: "ProductVariation",
                principalColumn: "ProductVariationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_ProductVariation_ProductVariationId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Promotion_PromotionId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_ProductVariation_ProductVariationId",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderProduct_ProductVariationId",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_Cart_ProductVariationId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ProductVariationId",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "ProductVariationId",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "PromotionName",
                table: "Promotion",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DoB",
                table: "Admin",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Promotion_PromotionId",
                table: "Order",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "PromotionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
