using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.CompanyConfiguration;

public class CompanyModelBuilderConfiguration : BaseModelBuilderConfiguration<Company>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Company> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(c => c.AboutUs)
            .IsRequired()
            .HasMaxLength(1500);

        builder.Property(c => c.WebSiteAddress)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ActivityType)
            .HasMaxLength(120);

        builder.Property(c => c.OwnershipType)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(25);

        builder.Property(c => c.CompanySize)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(25);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasOne(c => c.OwnedByUser)
            .WithOne(u => u.Company)
            .HasForeignKey<Company>(c => c.OwnedByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.CompanyCities)
            .WithOne(cc => cc.Company)
            .HasForeignKey(cc => cc.CompanyId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasMany(c => c.Advertisements)
            .WithOne(a => a.Company)
            .HasForeignKey(a => a.CompanyId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(c => c.CompanyImageFile)
            .WithOne()
            .HasForeignKey<Company>(c => c.CompanyImageFileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
