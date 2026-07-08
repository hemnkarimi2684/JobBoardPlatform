using JobBoardPlatform.Core.Entities.Common.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.Common;

public abstract class BaseModelBuilderConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasIndex(b => b.CreatedAt);

        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(b => !b.IsDeleted && b.DeletedAt == null);

        ApplyEntityConfiguration(builder);

    }

    protected abstract void ApplyEntityConfiguration(EntityTypeBuilder<T> builder);
}

