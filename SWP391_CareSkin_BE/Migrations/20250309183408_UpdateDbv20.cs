using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Customers_CustomerId",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_CustomerId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "History");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "History",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_History_CustomerId",
                table: "History",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Customers_CustomerId",
                table: "History",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }
    }
}
