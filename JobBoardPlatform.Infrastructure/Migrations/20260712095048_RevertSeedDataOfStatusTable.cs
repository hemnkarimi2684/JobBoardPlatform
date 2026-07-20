using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RevertSeedDataOfStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "CreatedById", "DeletedAt", "DeletedById", "Description", "ModifiedAt", "ModifiedById", "Title" },
                values: new object[,]
                {
                    { new Guid("0d835f3e-26e1-4390-9690-1ad13cd448f4"), null, null, null, "The job request is currently under review.", null, null, "Reviewing" },
                    { new Guid("1ac04d7f-faaa-4a32-a5eb-2be9cf0e551f"), null, null, null, "The candidate has been invited to an interview.", null, null, "Interview" },
                    { new Guid("b930e70d-3f8f-44a3-a48a-d80f351b9e6b"), null, null, null, "The job request is waiting for initial processing.", null, null, "Pending" },
                    { new Guid("ea15f434-f775-4399-913b-9b723b3998f7"), null, null, null, "The job request has been accepted.", null, null, "Accepted" },
                    { new Guid("f85ed918-8c7d-4418-bb58-aa8c9126b67c"), null, null, null, "The job request has been rejected.", null, null, "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("0d835f3e-26e1-4390-9690-1ad13cd448f4"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("1ac04d7f-faaa-4a32-a5eb-2be9cf0e551f"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("b930e70d-3f8f-44a3-a48a-d80f351b9e6b"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("ea15f434-f775-4399-913b-9b723b3998f7"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("f85ed918-8c7d-4418-bb58-aa8c9126b67c"));
        }
    }
}
