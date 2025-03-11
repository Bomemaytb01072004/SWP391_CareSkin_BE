using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Result_Quiz_QuizId",
                table: "Result");

            migrationBuilder.DropIndex(
                name: "IX_Result_QuizId",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Result");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "Result",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Result_QuizId",
                table: "Result",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Result_Quiz_QuizId",
                table: "Result",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
