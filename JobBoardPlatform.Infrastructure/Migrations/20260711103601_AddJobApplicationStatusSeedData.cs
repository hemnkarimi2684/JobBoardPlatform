using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJobApplicationStatusSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "DeletedAt", "Description", "ModifiedAt", "Title" },
                values: new object[,]
                {
                    { new Guid("0eeedafd-5374-47ce-a886-b0164fcbf5b7"), null, "The job request has been rejected.", null, "Rejected" },
                    { new Guid("4c349779-8b90-463e-89e2-46998b714bda"), null, "The candidate has been invited to an interview.", null, "Interview" },
                    { new Guid("52e1c539-a8c2-425e-a275-2b745946c1c1"), null, "The job request is currently under review.", null, "Reviewing" },
                    { new Guid("97da4cfc-8a13-4176-a31c-d0aaa6efefe7"), null, "The job request has been accepted.", null, "Accepted" },
                    { new Guid("b603c64c-4b9d-4bf8-a41f-5a739c7a054c"), null, "The job request is waiting for initial processing.", null, "Pending" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("0eeedafd-5374-47ce-a886-b0164fcbf5b7"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("4c349779-8b90-463e-89e2-46998b714bda"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("52e1c539-a8c2-425e-a275-2b745946c1c1"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("97da4cfc-8a13-4176-a31c-d0aaa6efefe7"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("b603c64c-4b9d-4bf8-a41f-5a739c7a054c"));
        }
    }
}
