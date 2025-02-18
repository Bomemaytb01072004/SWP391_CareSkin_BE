using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ingredient_name",
                table: "ProductMainIngredient",
                newName: "IngredientName");

            migrationBuilder.RenameColumn(
                name: "Ingredient_name",
                table: "ProductDetailIngredient",
                newName: "IngredientName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IngredientName",
                table: "ProductMainIngredient",
                newName: "Ingredient_name");

            migrationBuilder.RenameColumn(
                name: "IngredientName",
                table: "ProductDetailIngredient",
                newName: "Ingredient_name");
        }
    }
}
