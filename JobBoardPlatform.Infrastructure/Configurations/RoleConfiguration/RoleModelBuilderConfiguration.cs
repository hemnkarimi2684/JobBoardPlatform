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
    }
}
