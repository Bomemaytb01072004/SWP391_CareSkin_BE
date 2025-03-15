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
            migrationBuilder.DropForeignKey(
                name: "FK_BlogNew_Customers_CustomerId",
                table: "BlogNew");

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

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "BlogNew",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "BlogNew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BlogNew",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "BlogNew",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VnpayTransactions",
                table: "VnpayTransactions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogNew_AdminId",
                table: "BlogNew",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogNew_StaffId",
                table: "BlogNew",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogNew_Admin_AdminId",
                table: "BlogNew",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogNew_Customers_CustomerId",
                table: "BlogNew",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogNew_Staff_StaffId",
                table: "BlogNew",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "StaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogNew_Admin_AdminId",
                table: "BlogNew");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogNew_Customers_CustomerId",
                table: "BlogNew");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogNew_Staff_StaffId",
                table: "BlogNew");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VnpayTransactions",
                table: "VnpayTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BlogNew_AdminId",
                table: "BlogNew");

            migrationBuilder.DropIndex(
                name: "IX_BlogNew_StaffId",
                table: "BlogNew");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Staff");

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
                name: "AdminId",
                table: "BlogNew");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BlogNew");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "BlogNew");

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

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "BlogNew",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VnpayTransactions",
                table: "VnpayTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogNew_Customers_CustomerId",
                table: "BlogNew",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
