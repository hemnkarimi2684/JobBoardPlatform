using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;

namespace JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;

/// <summary>
/// دسته بندی شغل 
/// </summary>
public class JobCategory : BaseEntity
{
    private JobCategory() { }

    public JobCategory(string name, Guid? createdById = null)
    {
        Name = name;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// اسم دسته بندی 
    /// </summary>
    public string Name { get; private set; }

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به کار هایی که در این دسته بندی  شغلی هستند 
    /// </summary>
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    /// <summary>
    /// جزئیات مربوط به شرکت هایی که در این دسته بندی شغلی وجود دارند 
    /// </summary>
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(DomainErrors.JobCategoryNameIsRequired);

        if (Name.Length < 2 || Name.Length > 150)
            throw new DomainException(DomainErrors.JobCategoryNameInvalidLength);
    }
}
