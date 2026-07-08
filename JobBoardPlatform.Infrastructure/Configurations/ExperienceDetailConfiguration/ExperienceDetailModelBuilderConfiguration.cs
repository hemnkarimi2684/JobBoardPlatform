using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.ExperienceDetailConfiguration;

public class ExperienceDetailModelBuilderConfiguration : BaseModelBuilderConfiguration<ExperienceDetail>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<ExperienceDetail> builder)
    {
        builder.Property(ed => ed.LastJobTitle)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(ed => ed.JobCategory)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(ed => ed.City)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(p => p.SeniorityLevel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(25);
    }
}
