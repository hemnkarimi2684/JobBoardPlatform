using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.JobApplicationConfiguration;

public class JobApplicationModelBuilderConfiguration : BaseModelBuilderConfiguration<JobApplication>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<JobApplication> builder)
    {
        builder.Property(ja => ja.JobTitle)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ja => ja.CompanyName)
             .IsRequired()
             .HasMaxLength(120);

        builder.Property(ja => ja.CityName)
             .IsRequired()
             .HasMaxLength(120);

        builder.Property(ja => ja.UserFullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ja => ja.Status)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(25);

        builder.Property(ja => ja.CollaborationType)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(25);

        builder.HasOne(ja => ja.Advertisement)
            .WithMany(a => a.JobApplications)
            .HasForeignKey(ja => ja.AdvertisementId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
