using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttemptNumber",
                table: "UserQuizAttempt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "UserQuizAttempt",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserQuizAttempt",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "UserQuizAttempt",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Result",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "Result",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserQuizAttemptId",
                table: "Result",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Result_UserQuizAttemptId",
                table: "Result",
                column: "UserQuizAttemptId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Result_UserQuizAttempt_UserQuizAttemptId",
                table: "Result",
                column: "UserQuizAttemptId",
                principalTable: "UserQuizAttempt",
                principalColumn: "UserQuizAttemptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Result_UserQuizAttempt_UserQuizAttemptId",
                table: "Result");

            migrationBuilder.DropIndex(
                name: "IX_Result_UserQuizAttemptId",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "AttemptNumber",
                table: "UserQuizAttempt");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "UserQuizAttempt");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserQuizAttempt");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "UserQuizAttempt");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "UserQuizAttemptId",
                table: "Result");
        }
    }
}
