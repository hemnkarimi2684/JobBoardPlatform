using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviorBetweenResumeAndAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Attachments_LastUploadedFileId",
                table: "Resumes");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Attachments_LastUploadedFileId",
                table: "Resumes",
                column: "LastUploadedFileId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Attachments_LastUploadedFileId",
                table: "Resumes");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Attachments_LastUploadedFileId",
                table: "Resumes",
                column: "LastUploadedFileId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }
    }
}
