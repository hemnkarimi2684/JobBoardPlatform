using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Extensions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;

namespace JobBoardPlatform.Core.Entities.JobEntity.Entity;

/// <summary>
/// شغل
/// </summary>
public class Job : BaseEntity
{
    private Job() { }
    
    public Job(string name, Guid? createdById = null)
    {
        Name = name;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// اسم شغل
    /// </summary>
    public string Name { get; private set; }

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به اگهی های مربوط به شغل 
    /// </summary>
    public virtual ICollection<Advertisement> Advertisements { get; private set; } = new List<Advertisement>();

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(DomainErrors.JobNameIsRequired);

        if (Name.Length < 2 || Name.Length > 100)
            throw new DomainException(DomainErrors.JobNameInvalidLength);

        Name.IsAllLetter();
    }
}
