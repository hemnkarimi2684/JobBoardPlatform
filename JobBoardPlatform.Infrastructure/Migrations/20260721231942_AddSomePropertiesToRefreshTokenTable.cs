using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSomePropertiesToRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCategory_Users_CreatedById",
                table: "JobCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCategory_Users_DeletedById",
                table: "JobCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCategory_Users_ModifiedById",
                table: "JobCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobCategory_JobCategoryId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_CreatedById",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_DeletedById",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_ModifiedById",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCategory",
                table: "JobCategory");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "JobCategory",
                newName: "JobCategories");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_ModifiedById",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_DeletedById",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_CreatedById",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_CreatedAt",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategory_Name",
                table: "JobCategories",
                newName: "IX_JobCategories_Name");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategory_ModifiedById",
                table: "JobCategories",
                newName: "IX_JobCategories_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategory_DeletedById",
                table: "JobCategories",
                newName: "IX_JobCategories_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategory_CreatedById",
                table: "JobCategories",
                newName: "IX_JobCategories_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategory_CreatedAt",
                table: "JobCategories",
                newName: "IX_JobCategories_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCategories",
                table: "JobCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCategories_Users_CreatedById",
                table: "JobCategories",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCategories_Users_DeletedById",
                table: "JobCategories",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCategories_Users_ModifiedById",
                table: "JobCategories",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobCategories_JobCategoryId",
                table: "Jobs",
                column: "JobCategoryId",
                principalTable: "JobCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_CreatedById",
                table: "RefreshTokens",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_DeletedById",
                table: "RefreshTokens",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_ModifiedById",
                table: "RefreshTokens",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCategories_Users_CreatedById",
                table: "JobCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCategories_Users_DeletedById",
                table: "JobCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCategories_Users_ModifiedById",
                table: "JobCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobCategories_JobCategoryId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_CreatedById",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_DeletedById",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_ModifiedById",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCategories",
                table: "JobCategories");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "JobCategories",
                newName: "JobCategory");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_ModifiedById",
                table: "RefreshToken",
                newName: "IX_RefreshToken_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_DeletedById",
                table: "RefreshToken",
                newName: "IX_RefreshToken_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_CreatedById",
                table: "RefreshToken",
                newName: "IX_RefreshToken_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_CreatedAt",
                table: "RefreshToken",
                newName: "IX_RefreshToken_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategories_Name",
                table: "JobCategory",
                newName: "IX_JobCategory_Name");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategories_ModifiedById",
                table: "JobCategory",
                newName: "IX_JobCategory_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategories_DeletedById",
                table: "JobCategory",
                newName: "IX_JobCategory_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategories_CreatedById",
                table: "JobCategory",
                newName: "IX_JobCategory_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_JobCategories_CreatedAt",
                table: "JobCategory",
                newName: "IX_JobCategory_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCategory",
                table: "JobCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCategory_Users_CreatedById",
                table: "JobCategory",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCategory_Users_DeletedById",
                table: "JobCategory",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCategory_Users_ModifiedById",
                table: "JobCategory",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobCategory_JobCategoryId",
                table: "Jobs",
                column: "JobCategoryId",
                principalTable: "JobCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_CreatedById",
                table: "RefreshToken",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_DeletedById",
                table: "RefreshToken",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_ModifiedById",
                table: "RefreshToken",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
