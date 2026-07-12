using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.AspNetCore.Identity;

namespace JobBoardPlatform.Core.Entities.RoleEntity.Entity;

public class Role : IdentityRole<Guid>, IEntity
{
    private Role() { }

    public Role(string name,string? description = null, Guid? createdById = null)
    {
        Name = name;
        Description = description;
        CreatedById = createdById;

        Validate();
    }

    public string? Description { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    #region Foreign Keys

    public Guid? CreatedById { get; private set; }

    public Guid? ModifiedById { get; private set; }

    public Guid? DeletedById { get; private set; }

    #endregion

    #region Navigation Properties

    public User? Creator { get; private set; }

    public User? Modifier { get; private set; }

    public User? Deleter { get; private set; }

    #endregion

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(DomainErrors.RoleNameInvalidLength);

        if (Name.Length < 2 || Name.Length > 100)
            throw new DomainException(DomainErrors.RoleNameInvalidLength);

        if (Description is not null)
        {
            if (Description.Length < 2 || Description.Length > 100)
                throw new DomainException(DomainErrors.StatusDescriptionInvalidLength);
        }
    }

    public void SoftDelete(Guid deletedById)
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        ModifiedAt = DateTime.UtcNow;
        DeletedById = deletedById;
    }

    public void Update(Guid modifiedById)
    {
        ModifiedById = modifiedById;
        ModifiedAt = DateTime.UtcNow;
    }
}
