using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;

public class UpdateUserProfile
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Bio { get; set; }

    public string? Address { get; set; }

    public DateTime? BirthDate { get; set; }

    public Guid? CityId { get; set; }

    public Gender? Gender { get; set; }

    public Guid? ModifiedById { get; set; }
}

