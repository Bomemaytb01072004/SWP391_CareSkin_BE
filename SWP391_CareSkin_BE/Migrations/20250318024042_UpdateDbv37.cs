using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Order");
        }
    }
}
