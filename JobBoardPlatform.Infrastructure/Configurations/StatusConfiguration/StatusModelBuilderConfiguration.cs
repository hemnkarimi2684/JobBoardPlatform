using JobBoardPlatform.Core.Entities.StatusEntity.Entity;
using JobBoardPlatform.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoardPlatform.Infrastructure.Configurations.StatusConfiguration;

public class StatusModelBuilderConfiguration : BaseModelBuilderConfiguration<Status>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Status> builder)
    {
        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany(s => s.JobApplications)
            .WithOne(ja => ja.Status)
            .HasForeignKey(ja => ja.StatusId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        //builder.HasData(
        //    new Status("Pending", "The job request is waiting for initial processing.")
        //    {
        //        Id = new Guid("b930e70d-3f8f-44a3-a48a-d80f351b9e6b")
        //    },
        //    new Status("Reviewing", "The job request is currently under review.")
        //    {
        //        Id = new Guid("0d835f3e-26e1-4390-9690-1ad13cd448f4")
        //    },
        //    new Status("Interview", "The candidate has been invited to an interview.")
        //    {
        //        Id = new Guid("1ac04d7f-faaa-4a32-a5eb-2be9cf0e551f")
        //    },
        //    new Status("Rejected", "The job request has been rejected.")
        //    {
        //        Id = new Guid("f85ed918-8c7d-4418-bb58-aa8c9126b67c")
        //    },
        //    new Status("Accepted", "The job request has been accepted.")
        //    {
        //        Id = new Guid("ea15f434-f775-4399-913b-9b723b3998f7")
        //    }
        //    );
    }
}
