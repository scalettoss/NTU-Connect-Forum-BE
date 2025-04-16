using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumBE.Migrations
{
    /// <inheritdoc />
    public partial class refactoring_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_UploadedAt",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "Attachments",
                newName: "UpdatdedAt");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Attachments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CategoryId",
                table: "Attachments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CreatedAt",
                table: "Attachments",
                column: "CreatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Categories_CategoryId",
                table: "Attachments",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Categories_CategoryId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_CategoryId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_CreatedAt",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "UpdatdedAt",
                table: "Attachments",
                newName: "UploadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadedAt",
                table: "Attachments",
                column: "UploadedAt");
        }
    }
}
