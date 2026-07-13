namespace JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Command;

public record CreateJobApplicationCommand(
    Guid ResumeId, 
    Guid AdvertisementId,
    Guid UserId
);

