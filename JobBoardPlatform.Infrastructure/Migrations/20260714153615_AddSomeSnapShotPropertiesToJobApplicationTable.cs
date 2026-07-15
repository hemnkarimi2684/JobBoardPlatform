using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeSnapShotPropertiesToJobApplicationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "JobApplications",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CollaborationType",
                table: "JobApplications",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "JobApplications",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExperienceLevel",
                table: "JobApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "JobApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserFullName",
                table: "JobApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CityName",
                table: "JobApplications",
                column: "CityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CompanyName",
                table: "JobApplications",
                column: "CompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobTitle",
                table: "JobApplications",
                column: "JobTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name",
                table: "Cities",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobApplications_CityName",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_CompanyName",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_JobTitle",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CollaborationType",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "UserFullName",
                table: "JobApplications");
        }
    }
}
