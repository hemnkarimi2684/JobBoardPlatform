using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.JobApplicationConfiguration;

public class JobApplicationModelBuilderConfiguration : BaseModelBuilderConfiguration<JobApplication>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<JobApplication> builder)
    {
        builder.Property(a => a.Status)
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
