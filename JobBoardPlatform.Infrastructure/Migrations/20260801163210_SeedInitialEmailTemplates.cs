using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialEmailTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "Body", "CreatedById", "DeletedAt", "DeletedById", "IsActive", "Key", "ModifiedAt", "ModifiedById", "Subject" },
                values: new object[,]
                {
                    { new Guid("41f80e91-65d4-4b1a-9c19-000000000001"), "Your employer account has been approved.", null, null, null, true, "EmployerApproved", null, null, "Employer Account Approved" },
                    { new Guid("41f80e91-65d4-4b1a-9c19-000000000002"), "Unfortunately, your employer account has not been approved. Please contact support for more information.", null, null, null, true, "EmployerRejected", null, null, "Employer Account Rejected" },
                    { new Guid("41f80e91-65d4-4b1a-9c19-000000000003"), "You have received a new application for the {{JobTitle}} position.\n\nPlease log in to your dashboard to review the application.", null, null, null, true, "NewJobApplicationReceived", null, null, "New Application for {{JobTitle}}" },
                    { new Guid("41f80e91-65d4-4b1a-9c19-000000000004"), "Hello {{CandidateName}},\n\nYour application for the {{JobTitle}} position at {{CompanyName}} is currently under review.", null, null, null, true, "JobApplicationReviewing", null, null, "Your Job Application Is Under Review" },
                    { new Guid("41f80e91-65d4-4b1a-9c19-000000000005"), "Hello {{CandidateName}},\n\nYou have been invited to an interview for the {{JobTitle}} position at {{CompanyName}}.\n\nPlease log in to your account to view the interview details.", null, null, null, true, "JobApplicationInterview", null, null, "Interview Invitation" },
                    { new Guid("41f80e91-65d4-4b1a-9c19-000000000006"), "Hello {{CandidateName}},\n\nCongratulations! Your application for the {{JobTitle}} position at {{CompanyName}} has been accepted.", null, null, null, true, "JobApplicationAccepted", null, null, "Your Job Application Has Been Accepted" },
                    { new Guid("41f80e91-65d4-4b1a-9c19-000000000007"), "Hello {{CandidateName}},\n\nUnfortunately, your application for the {{JobTitle}} position at {{CompanyName}} was not accepted.\n\nWe wish you success in your future applications.", null, null, null, true, "JobApplicationRejected", null, null, "Job Application Update" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41f80e91-65d4-4b1a-9c19-000000000001"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41f80e91-65d4-4b1a-9c19-000000000002"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41f80e91-65d4-4b1a-9c19-000000000003"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41f80e91-65d4-4b1a-9c19-000000000004"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41f80e91-65d4-4b1a-9c19-000000000005"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41f80e91-65d4-4b1a-9c19-000000000006"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41f80e91-65d4-4b1a-9c19-000000000007"));
        }
    }
}
