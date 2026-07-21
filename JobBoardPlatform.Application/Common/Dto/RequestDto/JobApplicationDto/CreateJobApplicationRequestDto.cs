using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;

public class CreateJobApplicationRequestDto
{
    public Guid ResumeId { get; set; }

    public Guid AdvertisementId { get; set; }

    public Guid UserId { get; set; }
}

