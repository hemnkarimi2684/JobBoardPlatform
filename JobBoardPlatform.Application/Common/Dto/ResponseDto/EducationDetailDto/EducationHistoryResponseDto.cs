using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;

public class EducationHistoryResponseDto
{
    public Guid EducationDetailId { get; init; }

    public Guid UserId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CertificateDegree CertificateDegreeName { get; init; }

    public string Major { get; init; } = string.Empty;

    public string University { get; init; } = string.Empty;

    public DateTime StartDate { get; init; }

    public DateTime? CompletionDate { get; init; }

    public double? Percentage { get; init; }

    public bool IsCurrentlyStudying { get; init; }
}

