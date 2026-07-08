using JobBoardPlatform.Core.Entities.JobEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.JobConfiguration;

public class JobModelBuilderConfiguration : BaseModelBuilderConfiguration<Job>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Job> builder)
    {
        builder.Property(j => j.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(j => j.Advertisements)
            .WithOne(a => a.Job)
            .HasForeignKey(j => j.JobId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
