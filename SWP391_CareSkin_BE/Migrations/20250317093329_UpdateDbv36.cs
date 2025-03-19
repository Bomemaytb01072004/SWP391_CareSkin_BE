using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "MomoPayment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraData",
                table: "MomoCallback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderInfo",
                table: "MomoCallback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "MomoCallback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartnerCode",
                table: "MomoCallback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayType",
                table: "MomoCallback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ResponseTime",
                table: "MomoCallback",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TransId",
                table: "MomoCallback",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "MomoPayment");

            migrationBuilder.DropColumn(
                name: "ExtraData",
                table: "MomoCallback");

            migrationBuilder.DropColumn(
                name: "OrderInfo",
                table: "MomoCallback");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "MomoCallback");

            migrationBuilder.DropColumn(
                name: "PartnerCode",
                table: "MomoCallback");

            migrationBuilder.DropColumn(
                name: "PayType",
                table: "MomoCallback");

            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "MomoCallback");

            migrationBuilder.DropColumn(
                name: "TransId",
                table: "MomoCallback");
        }
    }
}
