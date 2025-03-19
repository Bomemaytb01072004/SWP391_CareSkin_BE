using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ProductVariation",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ProductForSkinType",
                columns: table => new
                {
                    ProductForSkinTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SkinTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductForSkinType", x => x.ProductForSkinTypeId);
                    table.ForeignKey(
                        name: "FK_ProductForSkinType_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductForSkinType_SkinType_SkinTypeId",
                        column: x => x.SkinTypeId,
                        principalTable: "SkinType",
                        principalColumn: "SkinTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductForSkinType_ProductId",
                table: "ProductForSkinType",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductForSkinType_SkinTypeId",
                table: "ProductForSkinType",
                column: "SkinTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductForSkinType");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "ProductVariation",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
