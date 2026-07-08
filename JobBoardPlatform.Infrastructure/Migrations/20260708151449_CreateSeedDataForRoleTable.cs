using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateSeedDataForRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "DeletedAt", "Description", "ModifiedAt", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6a9c6bfd-cb48-42d7-8d23-46f289461711"), null, null, "A company representative who creates job postings and reviews applicants.", null, "Employer", null },
                    { new Guid("9a8cfc1b-be14-42b8-bee4-0662d2a760e7"), null, null, "A system administrator who manages users, jobs, and platform settings.", null, "Admin", null },
                    { new Guid("a6eef362-f3cb-4bdf-a802-9f08b55ae7a9"), null, null, "A user who searches and applies for jobs.", null, "JobSeeker", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6a9c6bfd-cb48-42d7-8d23-46f289461711"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9a8cfc1b-be14-42b8-bee4-0662d2a760e7"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a6eef362-f3cb-4bdf-a802-9f08b55ae7a9"));
        }
    }
}
