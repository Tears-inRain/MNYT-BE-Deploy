using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleTemplateId",
                table: "ScheduleUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUsers_ScheduleTemplateId",
                table: "ScheduleUsers",
                column: "ScheduleTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleUsers_ScheduleTemplates_ScheduleTemplateId",
                table: "ScheduleUsers",
                column: "ScheduleTemplateId",
                principalTable: "ScheduleTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleUsers_ScheduleTemplates_ScheduleTemplateId",
                table: "ScheduleUsers");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleUsers_ScheduleTemplateId",
                table: "ScheduleUsers");

            migrationBuilder.DropColumn(
                name: "ScheduleTemplateId",
                table: "ScheduleUsers");
        }
    }
}
