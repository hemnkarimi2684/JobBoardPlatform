using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.CompanyCityConfiguration;

public class CompanyCityModelBuilderConfiguration : BaseModelBuilderConfiguration<CompanyCity>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<CompanyCity> builder)
    {
        builder.HasIndex(x => new { x.CompanyId, x.CityId })
             .IsUnique();

        builder.Property(cc => cc.Location)
            .IsRequired()
            .HasMaxLength(200);
    }
}
