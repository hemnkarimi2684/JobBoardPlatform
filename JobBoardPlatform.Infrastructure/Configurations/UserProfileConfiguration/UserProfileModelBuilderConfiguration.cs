using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.UserProfileConfiguration;

public class UserProfileModelBuilderConfiguration : BaseModelBuilderConfiguration<UserProfile>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(up => up.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(up => up.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(up => up.Bio)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(up => up.Address)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(up => up.Gender)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(25);

        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(up => up.City)
            .WithMany()
            .HasForeignKey(up => up.CityId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(up => up.UserImageFile)
            .WithOne()
            .HasForeignKey<UserProfile>(up => up.UserImageFileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
