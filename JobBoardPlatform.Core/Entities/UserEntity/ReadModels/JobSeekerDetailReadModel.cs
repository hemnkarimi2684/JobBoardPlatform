namespace JobBoardPlatform.Core.Entities.UserEntity.ReadModels;

public class JobSeekerDetailReadModel
{
    public Guid Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public int TotalCount { get; set; }
}
