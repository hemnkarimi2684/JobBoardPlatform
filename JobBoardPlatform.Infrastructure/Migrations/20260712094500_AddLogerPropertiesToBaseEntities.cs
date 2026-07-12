using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLogerPropertiesToBaseEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "UserSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "UserSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "UserSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Statuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Statuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Statuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Skills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Skills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Skills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Resumes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Resumes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Resumes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Provinces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Provinces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Provinces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "ExperienceDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "ExperienceDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "ExperienceDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "EducationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "EducationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "EducationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CompanyCities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "CompanyCities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "CompanyCities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Cities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Cities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Cities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "AdvertisementSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "AdvertisementSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "AdvertisementSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Advertisements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Advertisements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Advertisements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6a9c6bfd-cb48-42d7-8d23-46f289461711"),
                columns: new[] { "CreatedById", "DeletedById", "ModifiedById" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9a8cfc1b-be14-42b8-bee4-0662d2a760e7"),
                columns: new[] { "CreatedById", "DeletedById", "ModifiedById" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a6eef362-f3cb-4bdf-a802-9f08b55ae7a9"),
                columns: new[] { "CreatedById", "DeletedById", "ModifiedById" },
                values: new object[] { null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_CreatedById",
                table: "UserSkills",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_DeletedById",
                table: "UserSkills",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_ModifiedById",
                table: "UserSkills",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedById",
                table: "Users",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedById",
                table: "Users",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CreatedById",
                table: "UserProfiles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DeletedById",
                table: "UserProfiles",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ModifiedById",
                table: "UserProfiles",
                column: "ModifiedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CreatedById",
                table: "Skills",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_DeletedById",
                table: "Skills",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ModifiedById",
                table: "Skills",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DeletedById",
                table: "Roles",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModifiedById",
                table: "Roles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_CreatedById",
                table: "Resumes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_DeletedById",
                table: "Resumes",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ModifiedById",
                table: "Resumes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_CreatedById",
                table: "Provinces",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_DeletedById",
                table: "Provinces",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_ModifiedById",
                table: "Provinces",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CreatedById",
                table: "Payment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_DeletedById",
                table: "Payment",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ModifiedById",
                table: "Payment",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CreatedById",
                table: "Jobs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DeletedById",
                table: "Jobs",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ModifiedById",
                table: "Jobs",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CreatedById",
                table: "JobApplications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_DeletedById",
                table: "JobApplications",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ModifiedById",
                table: "JobApplications",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceDetails_CreatedById",
                table: "ExperienceDetails",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceDetails_DeletedById",
                table: "ExperienceDetails",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceDetails_ModifiedById",
                table: "ExperienceDetails",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDetails_CreatedById",
                table: "EducationDetails",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDetails_DeletedById",
                table: "EducationDetails",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDetails_ModifiedById",
                table: "EducationDetails",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCities_CreatedById",
                table: "CompanyCities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCities_DeletedById",
                table: "CompanyCities",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCities_ModifiedById",
                table: "CompanyCities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CreatedById",
                table: "Companies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_DeletedById",
                table: "Companies",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ModifiedById",
                table: "Companies",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CreatedById",
                table: "Cities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_DeletedById",
                table: "Cities",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ModifiedById",
                table: "Cities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CreatedById",
                table: "Attachments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_DeletedById",
                table: "Attachments",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ModifiedById",
                table: "Attachments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSkills_CreatedById",
                table: "AdvertisementSkills",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSkills_DeletedById",
                table: "AdvertisementSkills",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSkills_ModifiedById",
                table: "AdvertisementSkills",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CreatedById",
                table: "Advertisements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_DeletedById",
                table: "Advertisements",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModifiedById",
                table: "Advertisements",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Users_CreatedById",
                table: "Advertisements",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Users_DeletedById",
                table: "Advertisements",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Users_ModifiedById",
                table: "Advertisements",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSkills_Users_CreatedById",
                table: "AdvertisementSkills",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSkills_Users_DeletedById",
                table: "AdvertisementSkills",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSkills_Users_ModifiedById",
                table: "AdvertisementSkills",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Users_CreatedById",
                table: "Attachments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Users_DeletedById",
                table: "Attachments",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Users_ModifiedById",
                table: "Attachments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Users_CreatedById",
                table: "Cities",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Users_DeletedById",
                table: "Cities",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Users_ModifiedById",
                table: "Cities",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_CreatedById",
                table: "Companies",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_DeletedById",
                table: "Companies",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_ModifiedById",
                table: "Companies",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCities_Users_CreatedById",
                table: "CompanyCities",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCities_Users_DeletedById",
                table: "CompanyCities",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCities_Users_ModifiedById",
                table: "CompanyCities",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDetails_Users_CreatedById",
                table: "EducationDetails",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDetails_Users_DeletedById",
                table: "EducationDetails",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDetails_Users_ModifiedById",
                table: "EducationDetails",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperienceDetails_Users_CreatedById",
                table: "ExperienceDetails",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperienceDetails_Users_DeletedById",
                table: "ExperienceDetails",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperienceDetails_Users_ModifiedById",
                table: "ExperienceDetails",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_CreatedById",
                table: "JobApplications",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_DeletedById",
                table: "JobApplications",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_ModifiedById",
                table: "JobApplications",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_CreatedById",
                table: "Jobs",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_DeletedById",
                table: "Jobs",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_ModifiedById",
                table: "Jobs",
                column: "ModifiedById",
                principalTable: "Users",
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
                name: "FK_Provinces_Users_CreatedById",
                table: "Provinces",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Users_DeletedById",
                table: "Provinces",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Users_ModifiedById",
                table: "Provinces",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Users_CreatedById",
                table: "Resumes",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Users_DeletedById",
                table: "Resumes",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Users_ModifiedById",
                table: "Resumes",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_CreatedById",
                table: "Roles",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_DeletedById",
                table: "Roles",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_ModifiedById",
                table: "Roles",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Users_CreatedById",
                table: "Skills",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Users_DeletedById",
                table: "Skills",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Users_ModifiedById",
                table: "Skills",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_Users_CreatedById",
                table: "Statuses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_Users_DeletedById",
                table: "Statuses",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_Users_ModifiedById",
                table: "Statuses",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_CreatedById",
                table: "UserProfiles",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_DeletedById",
                table: "UserProfiles",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_ModifiedById",
                table: "UserProfiles",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedById",
                table: "Users",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_DeletedById",
                table: "Users",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ModifiedById",
                table: "Users",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Users_CreatedById",
                table: "UserSkills",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Users_DeletedById",
                table: "UserSkills",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Users_ModifiedById",
                table: "UserSkills",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Users_CreatedById",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Users_DeletedById",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Users_ModifiedById",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSkills_Users_CreatedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSkills_Users_DeletedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSkills_Users_ModifiedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_CreatedById",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_DeletedById",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_ModifiedById",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Users_CreatedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Users_DeletedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Users_ModifiedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_CreatedById",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_DeletedById",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_ModifiedById",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCities_Users_CreatedById",
                table: "CompanyCities");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCities_Users_DeletedById",
                table: "CompanyCities");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCities_Users_ModifiedById",
                table: "CompanyCities");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationDetails_Users_CreatedById",
                table: "EducationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationDetails_Users_DeletedById",
                table: "EducationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationDetails_Users_ModifiedById",
                table: "EducationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperienceDetails_Users_CreatedById",
                table: "ExperienceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperienceDetails_Users_DeletedById",
                table: "ExperienceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperienceDetails_Users_ModifiedById",
                table: "ExperienceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_CreatedById",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_DeletedById",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_ModifiedById",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_CreatedById",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_DeletedById",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_ModifiedById",
                table: "Jobs");

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
                name: "FK_Provinces_Users_CreatedById",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Users_DeletedById",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Users_ModifiedById",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Users_CreatedById",
                table: "Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Users_DeletedById",
                table: "Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Users_ModifiedById",
                table: "Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_CreatedById",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_DeletedById",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_ModifiedById",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Users_CreatedById",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Users_DeletedById",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Users_ModifiedById",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_Users_CreatedById",
                table: "Statuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_Users_DeletedById",
                table: "Statuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_Users_ModifiedById",
                table: "Statuses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_CreatedById",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_DeletedById",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_ModifiedById",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedById",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_DeletedById",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ModifiedById",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Users_CreatedById",
                table: "UserSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Users_DeletedById",
                table: "UserSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Users_ModifiedById",
                table: "UserSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserSkills_CreatedById",
                table: "UserSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserSkills_DeletedById",
                table: "UserSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserSkills_ModifiedById",
                table: "UserSkills");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedById",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DeletedById",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ModifiedById",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_CreatedById",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_DeletedById",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_ModifiedById",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Statuses_CreatedById",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Statuses_DeletedById",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Statuses_ModifiedById",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CreatedById",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_DeletedById",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_ModifiedById",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_DeletedById",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ModifiedById",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_CreatedById",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_DeletedById",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_ModifiedById",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_CreatedById",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_DeletedById",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_ModifiedById",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Payment_CreatedById",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_DeletedById",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_ModifiedById",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CreatedById",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_DeletedById",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ModifiedById",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_CreatedById",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_DeletedById",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_ModifiedById",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_ExperienceDetails_CreatedById",
                table: "ExperienceDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExperienceDetails_DeletedById",
                table: "ExperienceDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExperienceDetails_ModifiedById",
                table: "ExperienceDetails");

            migrationBuilder.DropIndex(
                name: "IX_EducationDetails_CreatedById",
                table: "EducationDetails");

            migrationBuilder.DropIndex(
                name: "IX_EducationDetails_DeletedById",
                table: "EducationDetails");

            migrationBuilder.DropIndex(
                name: "IX_EducationDetails_ModifiedById",
                table: "EducationDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCities_CreatedById",
                table: "CompanyCities");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCities_DeletedById",
                table: "CompanyCities");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCities_ModifiedById",
                table: "CompanyCities");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CreatedById",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_DeletedById",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ModifiedById",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CreatedById",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_DeletedById",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ModifiedById",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_CreatedById",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_DeletedById",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ModifiedById",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisementSkills_CreatedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisementSkills_DeletedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisementSkills_ModifiedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_CreatedById",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_DeletedById",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModifiedById",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserSkills");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "UserSkills");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserSkills");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ExperienceDetails");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "ExperienceDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ExperienceDetails");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EducationDetails");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "EducationDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EducationDetails");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "AdvertisementSkills");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Advertisements");

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
    }
}
