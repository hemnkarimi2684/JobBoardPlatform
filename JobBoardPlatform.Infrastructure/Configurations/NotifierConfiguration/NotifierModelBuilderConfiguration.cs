using JobBoardPlatform.Core.Entities.NotifierEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.NotifierConfiguration;

public class NotifierModelBuilderConfiguration : BaseModelBuilderConfiguration<Notifier>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Notifier> builder)
    {
        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(n => n.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.NoticeType)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(25);

        builder.HasOne(n => n.RecipientUser)
            .WithMany()
            .HasForeignKey(n => n.RecipientUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
