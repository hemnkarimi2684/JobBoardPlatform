using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.UserSkillConfiguration;

public class UserSkillModelBuilderConfiguration : BaseModelBuilderConfiguration<UserSkill>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<UserSkill> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.SkillId })
             .IsUnique();
    }
}
