using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Constants;
using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.EmailTemplateConfiguration;

public class EmailTemplateModelBuilderConfiguration : BaseModelBuilderConfiguration<EmailTemplate>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.Property(et => et.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(et => et.Subject)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(et => et.Body)
            .IsRequired();

        builder.HasIndex(et => et.Key)
            .IsUnique();

        builder.HasIndex(et => et.Subject);

        builder.HasData(
        new
        {
            Id = EmailTemplateSeedData.EmployerApprovedId,
            Key = EmailTemplateKeys.EmployerApproved,
            Subject = "Employer Account Approved",
            Body = "Your employer account has been approved.",
            IsActive = true
        },
        new
        {
            Id = EmailTemplateSeedData.EmployerRejectedId,
            Key = EmailTemplateKeys.EmployerRejected,
            Subject = "Employer Account Rejected",
            Body = "Unfortunately, your employer account has not been approved. Please contact support for more information.",
            IsActive = true
        },
        new
        {
            Id = EmailTemplateSeedData.NewJobApplicationReceivedId,
            Key = EmailTemplateKeys.NewJobApplicationReceived,
            Subject = "New Application for {{JobTitle}}",
            Body = "You have received a new application for the {{JobTitle}} position.\n\nPlease log in to your dashboard to review the application.",
            IsActive = true
        },
        new
        {
            Id = EmailTemplateSeedData.JobApplicationReviewingId,
            Key = EmailTemplateKeys.JobApplicationReviewing,
            Subject = "Your Job Application Is Under Review",
            Body = "Hello {{CandidateName}},\n\nYour application for the {{JobTitle}} position at {{CompanyName}} is currently under review.",
            IsActive = true
        },
        new
        {
            Id = EmailTemplateSeedData.JobApplicationInterviewId,
            Key = EmailTemplateKeys.JobApplicationInterview,
            Subject = "Interview Invitation",
            Body = "Hello {{CandidateName}},\n\nYou have been invited to an interview for the {{JobTitle}} position at {{CompanyName}}.\n\nPlease log in to your account to view the interview details.",
            IsActive = true
        },
        new
        {
            Id = EmailTemplateSeedData.JobApplicationAcceptedId,
            Key = EmailTemplateKeys.JobApplicationAccepted,
            Subject = "Your Job Application Has Been Accepted",
            Body = "Hello {{CandidateName}},\n\nCongratulations! Your application for the {{JobTitle}} position at {{CompanyName}} has been accepted.",
            IsActive = true
        },
        new
        {
            Id = EmailTemplateSeedData.JobApplicationRejectedId,
            Key = EmailTemplateKeys.JobApplicationRejected,
            Subject = "Job Application Update",
            Body = "Hello {{CandidateName}},\n\nUnfortunately, your application for the {{JobTitle}} position at {{CompanyName}} was not accepted.\n\nWe wish you success in your future applications.",
            IsActive = true
        });
        }
}
