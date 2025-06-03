using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumBE.Migrations
{
    /// <inheritdoc />
    public partial class deletedfieldadminreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScamDetections_AdminReviewed",
                table: "ScamDetections");

            migrationBuilder.DropColumn(
                name: "AdminReviewed",
                table: "ScamDetections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdminReviewed",
                table: "ScamDetections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ScamDetections_AdminReviewed",
                table: "ScamDetections",
                column: "AdminReviewed");
        }
    }
}
