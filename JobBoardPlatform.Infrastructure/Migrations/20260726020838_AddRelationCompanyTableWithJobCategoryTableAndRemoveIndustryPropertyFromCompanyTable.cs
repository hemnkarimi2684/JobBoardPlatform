using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationCompanyTableWithJobCategoryTableAndRemoveIndustryPropertyFromCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Companies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<Guid>(
                name: "JobCategoryId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Companies_JobCategoryId",
                table: "Companies",
                column: "JobCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_JobCategories_JobCategoryId",
                table: "Companies",
                column: "JobCategoryId",
                principalTable: "JobCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_JobCategories_JobCategoryId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_JobCategoryId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "JobCategoryId",
                table: "Companies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Companies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
