using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.UserConfiguration;

public class UserModelBuilderConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasIndex(b => b.CreatedAt);

        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(b => !b.IsDeleted && b.DeletedAt == null);

        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasMany(u => u.EducationDetails)
            .WithOne(ed => ed.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasMany(u => u.ExperienceDetails)
            .WithOne(ed => ed.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(u => u.Resume)
            .WithOne(r => r.User)
            .HasForeignKey<Resume>(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.UserSkills)
            .WithOne(us => us.User)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasMany(u => u.JobApplications)
            .WithOne(ja => ja.User)
            .HasForeignKey(ja => ja.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(u => u.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Deleter)
            .WithMany()
            .HasForeignKey(u => u.DeletedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Modifier)
            .WithMany()
            .HasForeignKey(u => u.ModifiedById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
