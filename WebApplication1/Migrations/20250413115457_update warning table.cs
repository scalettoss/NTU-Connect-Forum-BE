using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumBE.Migrations
{
    /// <inheritdoc />
    public partial class updatewarningtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarningLevel",
                table: "Warnings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarningLevel",
                table: "Warnings");
        }
    }
}
