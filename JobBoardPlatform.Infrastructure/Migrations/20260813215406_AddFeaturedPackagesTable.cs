using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeaturedPackagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeaturedPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturedPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeaturedPackages_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeaturedPackages_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeaturedPackages_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedPackages_CreatedAt",
                table: "FeaturedPackages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedPackages_CreatedById",
                table: "FeaturedPackages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedPackages_DeletedById",
                table: "FeaturedPackages",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedPackages_DurationInDays",
                table: "FeaturedPackages",
                column: "DurationInDays",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedPackages_ModifiedById",
                table: "FeaturedPackages",
                column: "ModifiedById");

            migrationBuilder.InsertData(
                table: "FeaturedPackages",
                columns: new[] { "Id", "CreatedById", "DeletedAt", "DeletedById", "DurationInDays", "IsDeleted", "ModifiedAt", "ModifiedById", "Price" },
                values: new object[,]
                {
                    { new Guid("0a5e8f00-0000-4000-8000-000000000007"), null, null, null, 7, false, null, null, 50000m },
                    { new Guid("0a5e8f00-0000-4000-8000-000000000015"), null, null, null, 15, false, null, null, 90000m },
                    { new Guid("0a5e8f00-0000-4000-8000-000000000030"), null, null, null, 30, false, null, null, 150000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeaturedPackages",
                keyColumn: "Id",
                keyValue: new Guid("0a5e8f00-0000-4000-8000-000000000007"));

            migrationBuilder.DeleteData(
                table: "FeaturedPackages",
                keyColumn: "Id",
                keyValue: new Guid("0a5e8f00-0000-4000-8000-000000000015"));

            migrationBuilder.DeleteData(
                table: "FeaturedPackages",
                keyColumn: "Id",
                keyValue: new Guid("0a5e8f00-0000-4000-8000-000000000030"));

            migrationBuilder.DropTable(
                name: "FeaturedPackages");
        }
    }
}
