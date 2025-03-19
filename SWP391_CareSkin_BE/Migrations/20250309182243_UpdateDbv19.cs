using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Customers_CustomerId",
                table: "History");

            migrationBuilder.DropTable(
                name: "SkinCareRoutineProduct");

            migrationBuilder.DropTable(
                name: "SkinCareRoutine");

            migrationBuilder.RenameColumn(
                name: "QuestionContext",
                table: "Question",
                newName: "QuestionText");

            migrationBuilder.RenameColumn(
                name: "PointForSkinType",
                table: "Answer",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "AnswersContext",
                table: "Answer",
                newName: "AnswersText");

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "SkinType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinScore",
                table: "SkinType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalScore",
                table: "Result",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "History",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AttemmptId",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Routine",
                columns: table => new
                {
                    RoutineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoutineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoutinePeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkinTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routine", x => x.RoutineId);
                    table.ForeignKey(
                        name: "FK_Routine_SkinType_SkinTypeId",
                        column: x => x.SkinTypeId,
                        principalTable: "SkinType",
                        principalColumn: "SkinTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuizAttempt",
                columns: table => new
                {
                    UserQuizAttemptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    AttemptDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizAttempt", x => x.UserQuizAttemptId);
                    table.ForeignKey(
                        name: "FK_UserQuizAttempt_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuizAttempt_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutineProduct",
                columns: table => new
                {
                    RoutineProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoutineId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineProduct", x => x.RoutineProductId);
                    table.ForeignKey(
                        name: "FK_RoutineProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoutineProduct_Routine_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routine",
                        principalColumn: "RoutineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_History_AttemmptId",
                table: "History",
                column: "AttemmptId");

            migrationBuilder.CreateIndex(
                name: "IX_Routine_SkinTypeId",
                table: "Routine",
                column: "SkinTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineProduct_ProductId",
                table: "RoutineProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineProduct_RoutineId",
                table: "RoutineProduct",
                column: "RoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempt_CustomerId",
                table: "UserQuizAttempt",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempt_QuizId",
                table: "UserQuizAttempt",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Customers_CustomerId",
                table: "History",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_UserQuizAttempt_AttemmptId",
                table: "History",
                column: "AttemmptId",
                principalTable: "UserQuizAttempt",
                principalColumn: "UserQuizAttemptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Customers_CustomerId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_UserQuizAttempt_AttemmptId",
                table: "History");

            migrationBuilder.DropTable(
                name: "RoutineProduct");

            migrationBuilder.DropTable(
                name: "UserQuizAttempt");

            migrationBuilder.DropTable(
                name: "Routine");

            migrationBuilder.DropIndex(
                name: "IX_History_AttemmptId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "SkinType");

            migrationBuilder.DropColumn(
                name: "MinScore",
                table: "SkinType");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "AttemmptId",
                table: "History");

            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "Question",
                newName: "QuestionContext");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Answer",
                newName: "PointForSkinType");

            migrationBuilder.RenameColumn(
                name: "AnswersText",
                table: "Answer",
                newName: "AnswersContext");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SkinCareRoutine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkinTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinCareRoutine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkinCareRoutine_SkinType_SkinTypeId",
                        column: x => x.SkinTypeId,
                        principalTable: "SkinType",
                        principalColumn: "SkinTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkinCareRoutineProduct",
                columns: table => new
                {
                    SkinCareRoutineId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinCareRoutineProduct", x => new { x.SkinCareRoutineId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_SkinCareRoutineProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkinCareRoutineProduct_SkinCareRoutine_SkinCareRoutineId",
                        column: x => x.SkinCareRoutineId,
                        principalTable: "SkinCareRoutine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkinCareRoutine_SkinTypeId",
                table: "SkinCareRoutine",
                column: "SkinTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SkinCareRoutineProduct_ProductId",
                table: "SkinCareRoutineProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Customers_CustomerId",
                table: "History",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
