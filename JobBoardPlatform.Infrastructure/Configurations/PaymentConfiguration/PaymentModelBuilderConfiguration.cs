using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.PaymentConfiguration;

public class PaymentModelBuilderConfiguration : BaseModelBuilderConfiguration<Payment>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 4)
            .HasDefaultValue(0);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(25);

        builder.Property(p => p.DurationInDays)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(p => p.Advertisement)
            .WithMany(a => a.Payments)
            .HasForeignKey(p => p.AdvertisementId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
