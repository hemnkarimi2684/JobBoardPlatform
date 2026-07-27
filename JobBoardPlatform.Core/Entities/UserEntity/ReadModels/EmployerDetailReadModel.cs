namespace JobBoardPlatform.Core.Entities.UserEntity.ReadModels;

public class EmployerDetailReadModel
{
    public Guid EmployerId { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public DateTime EmployerCreatedAt { get; set; }

    public int TotalCount { get; set; }
}
