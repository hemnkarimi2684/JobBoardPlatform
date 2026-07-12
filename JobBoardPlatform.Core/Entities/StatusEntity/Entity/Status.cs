using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;

namespace JobBoardPlatform.Core.Entities.StatusEntity.Entity;

public class Status : BaseEntity
{
    private Status() { }
    
    public Status(string title, string description, Guid? createdById = null)
    {
        Title = title;
        Description = description;
        CreatedById = createdById;

        Validate();
    }

    public string Title { get; private set; }

    public string Description { get; private set; }

    #region Navigation Properties

    public virtual ICollection<JobApplication> JobApplications { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new DomainException(DomainErrors.StatusTitleIsRequired);

        if (Title.Length < 2 || Title.Length > 100)
            throw new DomainException(DomainErrors.StatusTitleInvalidLength);

        if (string.IsNullOrWhiteSpace(Description))
            throw new DomainException(DomainErrors.StatusDescriptionIsRequired);

        if (Description.Length < 2 || Description.Length > 150)
            throw new DomainException(DomainErrors.StatusDescriptionInvalidLength);
    }
}
