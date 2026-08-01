using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailTemplateTableToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifier_Users_CreatedById",
                table: "Notifier");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifier_Users_DeletedById",
                table: "Notifier");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifier_Users_ModifiedById",
                table: "Notifier");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifier_Users_RecipientUserId",
                table: "Notifier");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Advertisements_AdvertisementId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Users_CreatedById",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Users_DeletedById",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Users_ModifiedById",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Users_UserId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifier",
                table: "Notifier");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Notifier",
                newName: "Notifiers");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_UserId",
                table: "Payments",
                newName: "IX_Payments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_ModifiedById",
                table: "Payments",
                newName: "IX_Payments_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_DeletedById",
                table: "Payments",
                newName: "IX_Payments_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_CreatedById",
                table: "Payments",
                newName: "IX_Payments_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_CreatedAt",
                table: "Payments",
                newName: "IX_Payments_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_AdvertisementId",
                table: "Payments",
                newName: "IX_Payments_AdvertisementId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifier_RecipientUserId",
                table: "Notifiers",
                newName: "IX_Notifiers_RecipientUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifier_ModifiedById",
                table: "Notifiers",
                newName: "IX_Notifiers_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Notifier_DeletedById",
                table: "Notifiers",
                newName: "IX_Notifiers_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_Notifier_CreatedById",
                table: "Notifiers",
                newName: "IX_Notifiers_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Notifier_CreatedAt",
                table: "Notifiers",
                newName: "IX_Notifiers_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifiers",
                table: "Notifiers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_CreatedAt",
                table: "EmailTemplates",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_CreatedById",
                table: "EmailTemplates",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_DeletedById",
                table: "EmailTemplates",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_Key",
                table: "EmailTemplates",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_ModifiedById",
                table: "EmailTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_Subject",
                table: "EmailTemplates",
                column: "Subject");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifiers_Users_CreatedById",
                table: "Notifiers",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifiers_Users_DeletedById",
                table: "Notifiers",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifiers_Users_ModifiedById",
                table: "Notifiers",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifiers_Users_RecipientUserId",
                table: "Notifiers",
                column: "RecipientUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Advertisements_AdvertisementId",
                table: "Payments",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_CreatedById",
                table: "Payments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_DeletedById",
                table: "Payments",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_ModifiedById",
                table: "Payments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifiers_Users_CreatedById",
                table: "Notifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifiers_Users_DeletedById",
                table: "Notifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifiers_Users_ModifiedById",
                table: "Notifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifiers_Users_RecipientUserId",
                table: "Notifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Advertisements_AdvertisementId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_CreatedById",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_DeletedById",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_ModifiedById",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifiers",
                table: "Notifiers");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Notifiers",
                newName: "Notifier");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payment",
                newName: "IX_Payment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ModifiedById",
                table: "Payment",
                newName: "IX_Payment_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_DeletedById",
                table: "Payment",
                newName: "IX_Payment_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_CreatedById",
                table: "Payment",
                newName: "IX_Payment_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_CreatedAt",
                table: "Payment",
                newName: "IX_Payment_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_AdvertisementId",
                table: "Payment",
                newName: "IX_Payment_AdvertisementId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifiers_RecipientUserId",
                table: "Notifier",
                newName: "IX_Notifier_RecipientUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifiers_ModifiedById",
                table: "Notifier",
                newName: "IX_Notifier_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Notifiers_DeletedById",
                table: "Notifier",
                newName: "IX_Notifier_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_Notifiers_CreatedById",
                table: "Notifier",
                newName: "IX_Notifier_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Notifiers_CreatedAt",
                table: "Notifier",
                newName: "IX_Notifier_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifier",
                table: "Notifier",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifier_Users_CreatedById",
                table: "Notifier",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifier_Users_DeletedById",
                table: "Notifier",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifier_Users_ModifiedById",
                table: "Notifier",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifier_Users_RecipientUserId",
                table: "Notifier",
                column: "RecipientUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Advertisements_AdvertisementId",
                table: "Payment",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Users_CreatedById",
                table: "Payment",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Users_DeletedById",
                table: "Payment",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Users_ModifiedById",
                table: "Payment",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Users_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
