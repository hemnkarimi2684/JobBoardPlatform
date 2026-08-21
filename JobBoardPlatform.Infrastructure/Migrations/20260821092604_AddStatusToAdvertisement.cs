using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToAdvertisement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Advertisements",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "Open");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Status",
                table: "Advertisements",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_Status",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Advertisements");
        }
    }
}
