using JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.FeaturedPackageConfiguration;

public class FeaturedPackageModelBuilderConfiguration : BaseModelBuilderConfiguration<FeaturedPackage>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<FeaturedPackage> builder)
    {
        builder.Property(p => p.DurationInDays)
            .IsRequired();

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 4);

        builder.HasIndex(p => p.DurationInDays)
            .IsUnique();
    }
}
