using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.ProvinceConfiguration;

public class ProvinceModelBuilderConfiguration : BaseModelBuilderConfiguration<Province>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Province> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(p => p.ProvinceCode)
            .IsUnique();

        builder.HasMany(p => p.Cities)
             .WithOne(c => c.Province)
             .HasForeignKey(c => c.ProvinceId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired();
    }
}
