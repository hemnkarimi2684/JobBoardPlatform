using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleetBehaviorForAttachmentTableAndAddSomeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Attachments_CompanyImageFileId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Attachments_UserImageFileId",
                table: "UserProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Name",
                table: "Jobs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Attachments_CompanyImageFileId",
                table: "Companies",
                column: "CompanyImageFileId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Attachments_UserImageFileId",
                table: "UserProfiles",
                column: "UserImageFileId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Attachments_CompanyImageFileId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Attachments_UserImageFileId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_Name",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Name",
                table: "Companies");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Attachments_CompanyImageFileId",
                table: "Companies",
                column: "CompanyImageFileId",
                principalTable: "Attachments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Attachments_UserImageFileId",
                table: "UserProfiles",
                column: "UserImageFileId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }
    }
}
