using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.CityConfiguration;

public class CityModelBuilderConfiguration : BaseModelBuilderConfiguration<City>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<City> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.HasIndex(c => c.CityCode)
            .IsUnique();
    }
}
