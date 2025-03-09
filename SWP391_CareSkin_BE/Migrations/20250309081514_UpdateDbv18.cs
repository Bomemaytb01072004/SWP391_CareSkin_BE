using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_CareSkin_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbv18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "RatingFeedbackImage",
                newName: "FeedbackImageUrl");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RatingFeedbackImage",
                newName: "RatingFeedbackImageId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RatingFeedback",
                newName: "RatingFeedbackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedbackImageUrl",
                table: "RatingFeedbackImage",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "RatingFeedbackImageId",
                table: "RatingFeedbackImage",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RatingFeedbackId",
                table: "RatingFeedback",
                newName: "Id");
        }
    }
}
