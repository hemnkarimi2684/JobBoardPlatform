using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTheIndexesInJobApplicationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
