using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.RefreshTokenConfiguration;

public class RefreshTokenModelBuilderConfiguration : BaseModelBuilderConfiguration<RefreshToken>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasIndex(rt => rt.Token);

        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
