using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.StatusConfiguration;

public class StatusModelBuilderConfiguration : BaseModelBuilderConfiguration<Status>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Status> builder)
    {
        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany(s => s.JobApplications)
            .WithOne(ja => ja.Status)
            .HasForeignKey(ja => ja.StatusId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
