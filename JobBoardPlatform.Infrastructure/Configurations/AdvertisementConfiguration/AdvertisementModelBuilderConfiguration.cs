using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.AdvertisementConfiguration;

public class AdvertisementModelBuilderConfiguration : BaseModelBuilderConfiguration<Advertisement>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Advertisement> builder)
    {
        builder.Property(a => a.Description)
             .IsRequired()
             .HasMaxLength(2000);

        builder.Property(a => a.ExperienceLevel)
             .IsRequired()
             .HasMaxLength(100);

        builder.Property(a => a.MaximumSalary)
            .HasPrecision(18, 4);

        builder.Property(a => a.MinimumSalary)
            .HasPrecision(18, 4);

        builder.HasIndex(a => a.MinimumSalary);

        builder.HasIndex(a => a.MaximumSalary);

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        builder.Property(a => a.IsFeatured)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(a => a.FeaturedUntil);

        builder.HasMany(a => a.AdvertisementSkills)
            .WithOne(x => x.Advertisement)
            .HasForeignKey(x => x.AdvertisementId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(a => a.City)
            .WithMany()
            .HasForeignKey(a => a.CityId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
