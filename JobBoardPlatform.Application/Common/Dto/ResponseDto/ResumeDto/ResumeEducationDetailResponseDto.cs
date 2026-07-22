using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

public class ResumeEducationDetailResponseDto
{
    public Guid EducationDetailId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CertificateDegree CertificateDegreeName { get; init; }

    public string Major { get; init; } = string.Empty;

    public string University { get; init; } = string.Empty;

    public DateTime StartDate { get; init; }

    public DateTime? CompletionDate { get; init; }

    public double? Percentage { get; init; }

    public bool IsCurrentlyStudying { get; init; }
}