using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.AttachmentConfiguration;

public class AttachmentModelBuilderConfiguration : BaseModelBuilderConfiguration<Attachment>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Attachment> builder)
    {
        builder.Property(a => a.FileName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(a => a.ContentType)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(a => a.AttachmentType)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(25);
    }
}
