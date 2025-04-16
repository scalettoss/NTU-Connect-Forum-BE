using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumBE.Migrations
{
    /// <inheritdoc />
    public partial class cascade_report_reportstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportStatuses_Reports_ReportId",
                table: "ReportStatuses");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportStatuses_Reports_ReportId",
                table: "ReportStatuses",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "ReportId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportStatuses_Reports_ReportId",
                table: "ReportStatuses");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportStatuses_Reports_ReportId",
                table: "ReportStatuses",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "ReportId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
