using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using Microsoft.AspNetCore.Identity;

namespace JobBoardPlatform.Core.Entities.RoleEntity.Entity;

public class Role : IdentityRole<Guid>, IEntity
{
    private Role() { }

    public Role(string? description = null)
    {
        Description = description;

        Validate();
    }

    public string? Description { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Update() => ModifiedAt = DateTime.UtcNow;

    private void Validate()
    {
        if (Description is not null)
        {
            if (Description.Length < 2 || Description.Length > 100)
                throw new DomainException(DomainErrors.StatusDescriptionInvalidLength);
        }
    }
}
