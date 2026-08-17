using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;

public class UpdateEducationDetail
{
    public CertificateDegree? CertificateDegreeName { get; init; }

    public string? Major { get; init; }
    public string? University { get; init; }

    public DateTime? StartDate { get; init; }
    public DateTime? CompletionDate { get; init; }

    public double? Percentage { get; init; }

    public bool? IsCurrentlyStudying { get; init; }

    public Guid? ModifiedById { get; init; }
}

