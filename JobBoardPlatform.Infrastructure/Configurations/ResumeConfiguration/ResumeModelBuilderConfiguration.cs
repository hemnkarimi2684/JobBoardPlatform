using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.ResumeConfiguration;

public class ResumeModelBuilderConfiguration : BaseModelBuilderConfiguration<Resume>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Resume> builder)
    {
        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(r => r.JobApplications)
            .WithOne(ja => ja.Resume)
            .HasForeignKey(ja => ja.ResumeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(r => r.LastUploadedFile)
            .WithOne()
            .HasForeignKey<Resume>(r => r.LastUploadedFileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
