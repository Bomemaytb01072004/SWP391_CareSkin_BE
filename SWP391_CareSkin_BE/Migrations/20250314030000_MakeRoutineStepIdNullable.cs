using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class MakeRoutineStepIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineProduct_RoutineStep_RoutineStepId",
                table: "RoutineProduct");

            // Alter column to be nullable
            migrationBuilder.AlterColumn<int>(
                name: "RoutineStepId",
                table: "RoutineProduct",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Add back the foreign key with the nullable column
            migrationBuilder.AddForeignKey(
                name: "FK_RoutineProduct_RoutineStep_RoutineStepId",
                table: "RoutineProduct",
                column: "RoutineStepId",
                principalTable: "RoutineStep",
                principalColumn: "RoutineStepId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineProduct_RoutineStep_RoutineStepId",
                table: "RoutineProduct");

            // Change the column back to non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "RoutineStepId",
                table: "RoutineProduct",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Add back the foreign key with the non-nullable column
            migrationBuilder.AddForeignKey(
                name: "FK_RoutineProduct_RoutineStep_RoutineStepId",
                table: "RoutineProduct",
                column: "RoutineStepId",
                principalTable: "RoutineStep",
                principalColumn: "RoutineStepId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
