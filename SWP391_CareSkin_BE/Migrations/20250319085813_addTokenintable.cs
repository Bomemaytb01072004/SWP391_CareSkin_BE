using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class addTokenintable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "ResetPassword",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "ResetPassword");
        }
    }
}
