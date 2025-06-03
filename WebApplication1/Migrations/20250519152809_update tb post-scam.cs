using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumBE.Migrations
{
    /// <inheritdoc />
    public partial class updatetbpostscam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScamDetections_Posts_PostId",
                table: "ScamDetections");

            migrationBuilder.DropIndex(
                name: "IX_ScamDetections_PostId",
                table: "ScamDetections");

            migrationBuilder.CreateIndex(
                name: "IX_ScamDetections_PostId",
                table: "ScamDetections",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScamDetections_Posts_PostId",
                table: "ScamDetections",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScamDetections_Posts_PostId",
                table: "ScamDetections");

            migrationBuilder.DropIndex(
                name: "IX_ScamDetections_PostId",
                table: "ScamDetections");

            migrationBuilder.CreateIndex(
                name: "IX_ScamDetections_PostId",
                table: "ScamDetections",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScamDetections_Posts_PostId",
                table: "ScamDetections",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
