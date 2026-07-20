using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusTableInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Statuses_StatusId",
                table: "JobApplications");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_StatusId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "JobApplications");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "JobApplications",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "JobApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statuses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Statuses_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Statuses_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_StatusId",
                table: "JobApplications",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_CreatedAt",
                table: "Statuses",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_CreatedById",
                table: "Statuses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_DeletedById",
                table: "Statuses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_ModifiedById",
                table: "Statuses",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Statuses_StatusId",
                table: "JobApplications",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }
    }
}
