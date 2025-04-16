using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumBE.Migrations
{
    /// <inheritdoc />
    public partial class update_tb_userprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserProfiles",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserProfiles");
        }
    }
}
