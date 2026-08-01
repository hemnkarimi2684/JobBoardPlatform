using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.JobCategoryConfiguration;

public class JobCategoryModelBuilderConfiguration : BaseModelBuilderConfiguration<JobCategory>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<JobCategory> builder)
    {
        builder.Property(jc => jc.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(jc => jc.Name)
            .IsUnique();

        builder.HasMany(jc => jc.Jobs)
            .WithOne(j => j.JobCategory)
            .HasForeignKey(j => j.JobCategoryId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasMany(jc => jc.Companies)
            .WithOne(c => c.JobCategory)
            .HasForeignKey(c => c.JobCategoryId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
