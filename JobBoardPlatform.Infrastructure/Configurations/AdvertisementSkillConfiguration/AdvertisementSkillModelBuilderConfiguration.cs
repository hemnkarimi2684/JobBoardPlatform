using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.AdvertisementSkillConfiguration;

public class AdvertisementSkillModelBuilderConfiguration : BaseModelBuilderConfiguration<AdvertisementSkill>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<AdvertisementSkill> builder)
    {
        builder.HasIndex(x => new { x.AdvertisementId, x.SkillId })
            .IsUnique();
    }
}
