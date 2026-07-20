using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;

public record CreateJobApplicationRequestDto(
                                             [Required(ErrorMessage = "the ResumeId is required", AllowEmptyStrings = false)]
                                             Guid ResumeId,

                                             [Required(ErrorMessage = "the AdvertisementId is required", AllowEmptyStrings = false)]
                                             Guid AdvertisementId,

                                             [Required(ErrorMessage = "the UserId is required", AllowEmptyStrings = false)]
                                             Guid UserId
);

