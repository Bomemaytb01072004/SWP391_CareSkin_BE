using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPictures_Product_ProductId",
                table: "ProductPictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPictures",
                table: "ProductPictures");

            migrationBuilder.RenameTable(
                name: "ProductPictures",
                newName: "ProductPicture");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPictures_ProductId",
                table: "ProductPicture",
                newName: "IX_ProductPicture_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPicture",
                table: "ProductPicture",
                column: "ProductPictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPicture_Product_ProductId",
                table: "ProductPicture",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPicture_Product_ProductId",
                table: "ProductPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPicture",
                table: "ProductPicture");

            migrationBuilder.RenameTable(
                name: "ProductPicture",
                newName: "ProductPictures");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPicture_ProductId",
                table: "ProductPictures",
                newName: "IX_ProductPictures_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPictures",
                table: "ProductPictures",
                column: "ProductPictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPictures_Product_ProductId",
                table: "ProductPictures",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
