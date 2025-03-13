using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineProduct_Product_ProductId",
                table: "RoutineProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_RoutineProduct_Routine_RoutineId",
                table: "RoutineProduct");

            migrationBuilder.DropIndex(
                name: "IX_RoutineProduct_RoutineId",
                table: "RoutineProduct");

            migrationBuilder.AddColumn<int>(
                name: "RoutineStepId",
                table: "RoutineProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoutineStep",
                columns: table => new
                {
                    RoutineStepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoutineId = table.Column<int>(type: "int", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineStep", x => x.RoutineStepId);
                    table.ForeignKey(
                        name: "FK_RoutineStep_Routine_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routine",
                        principalColumn: "RoutineId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutineProduct_RoutineStepId",
                table: "RoutineProduct",
                column: "RoutineStepId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineStep_RoutineId",
                table: "RoutineStep",
                column: "RoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineProduct_Product_ProductId",
                table: "RoutineProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineProduct_RoutineStep_RoutineStepId",
                table: "RoutineProduct",
                column: "RoutineStepId",
                principalTable: "RoutineStep",
                principalColumn: "RoutineStepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineProduct_Product_ProductId",
                table: "RoutineProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_RoutineProduct_RoutineStep_RoutineStepId",
                table: "RoutineProduct");

            migrationBuilder.DropTable(
                name: "RoutineStep");

            migrationBuilder.DropIndex(
                name: "IX_RoutineProduct_RoutineStepId",
                table: "RoutineProduct");

            migrationBuilder.DropColumn(
                name: "RoutineStepId",
                table: "RoutineProduct");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineProduct_RoutineId",
                table: "RoutineProduct",
                column: "RoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineProduct_Product_ProductId",
                table: "RoutineProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineProduct_Routine_RoutineId",
                table: "RoutineProduct",
                column: "RoutineId",
                principalTable: "Routine",
                principalColumn: "RoutineId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
