using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateindexForSalaryRangeInAdvertisementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_MaximumSalary",
                table: "Advertisements",
                column: "MaximumSalary");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_MinimumSalary",
                table: "Advertisements",
                column: "MinimumSalary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_MaximumSalary",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_MinimumSalary",
                table: "Advertisements");
        }
    }
}
