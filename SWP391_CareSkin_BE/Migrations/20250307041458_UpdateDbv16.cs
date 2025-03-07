using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RatingFeedback",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "RatingFeedback",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RatingFeedback",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "RatingFeedbackImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingFeedbackId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingFeedbackImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingFeedbackImage_RatingFeedback_RatingFeedbackId",
                        column: x => x.RatingFeedbackId,
                        principalTable: "RatingFeedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RatingFeedbackImage_RatingFeedbackId",
                table: "RatingFeedbackImage",
                column: "RatingFeedbackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RatingFeedbackImage");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RatingFeedback");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "RatingFeedback");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RatingFeedback");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Product");
        }
    }
}
