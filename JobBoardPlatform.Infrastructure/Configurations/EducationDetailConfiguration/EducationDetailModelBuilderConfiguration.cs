using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.EducationDetailConfiguration;

public class EducationDetailModelBuilderConfiguration : BaseModelBuilderConfiguration<EducationDetail>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<EducationDetail> builder)
    {
        builder.Property(ed => ed.CertificateDegreeName)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(25);

        builder.Property(ed => ed.Major)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(ed => ed.University)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ed => ed.Percentage)
            .HasDefaultValue(0);
    }
}
