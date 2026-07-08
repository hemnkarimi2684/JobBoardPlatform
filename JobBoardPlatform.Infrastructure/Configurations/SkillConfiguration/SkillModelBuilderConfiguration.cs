using JobBoardPlatform.Core.Entities.SkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.SkillConfiguration;

public class SkillModelBuilderConfiguration : BaseModelBuilderConfiguration<Skill>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Skill> builder)
    {
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(s => s.AdvertisementSkills)
            .WithOne(x  => x.Skill)
            .HasForeignKey(x =>  x.SkillId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasMany(s => s.UserSkills)
            .WithOne(us => us.Skill)
            .HasForeignKey(us =>  us.SkillId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
