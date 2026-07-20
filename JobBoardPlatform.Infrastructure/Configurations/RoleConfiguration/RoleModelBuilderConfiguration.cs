using JobBoardPlatform.Core.Entities.RoleEntity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.RoleConfiguration;

public class RoleModelBuilderConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasIndex(b => b.CreatedAt);

        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(b => !b.IsDeleted && b.DeletedAt == null);

        builder.Property(r => r.Description)
            .HasMaxLength(100);

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

        var jobSekerId = new Guid("a6eef362-f3cb-4bdf-a802-9f08b55ae7a9");
        var adminId = new Guid("9a8cfc1b-be14-42b8-bee4-0662d2a760e7");
        var employerId = new Guid("6a9c6bfd-cb48-42d7-8d23-46f289461711");

        builder.HasData(
            new Role("JobSeeker", "A user who searches and applies for jobs.")
            {
                Id = jobSekerId,
            },
            new Role("Admin", "A system administrator who manages users, jobs, and platform settings.")
            {
                Id = adminId,
            },
            new Role("Employer", "A company representative who creates job postings and reviews applicants.")
            {
                Id = employerId,
            }
            );
    }
}
