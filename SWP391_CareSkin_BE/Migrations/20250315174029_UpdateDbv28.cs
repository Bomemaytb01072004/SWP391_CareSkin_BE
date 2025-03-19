using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VnpayTransactions",
                table: "VnpayTransactions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VnpayTransactions");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "VnpayTransactions",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "ResultCode",
                table: "VnpayTransactions",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "PayUrl",
                table: "VnpayTransactions",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "VnpayTransactions",
                newName: "OrderDescription");

            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "RatingFeedback",
                newName: "IsActive");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "VnpayTransactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Staff",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SkinType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Routine",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Quiz",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Brand",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BlogNews",
                type: "bit",
                nullable: false,
                defaultValue: false);


            migrationBuilder.AddPrimaryKey(
                name: "PK_VnpayTransactions",
                table: "VnpayTransactions",
                column: "TransactionId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropPrimaryKey(
                name: "PK_VnpayTransactions",
                table: "VnpayTransactions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SkinType");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Routine");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Brand");


            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BlogNews");


            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "VnpayTransactions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "VnpayTransactions",
                newName: "PayUrl");

            migrationBuilder.RenameColumn(
                name: "OrderDescription",
                table: "VnpayTransactions",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "VnpayTransactions",
                newName: "ResultCode");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "RatingFeedback",
                newName: "IsVisible");

            migrationBuilder.AlterColumn<int>(
                name: "ResultCode",
                table: "VnpayTransactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VnpayTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VnpayTransactions",
                table: "VnpayTransactions",
                column: "Id");
        }
    }
}
