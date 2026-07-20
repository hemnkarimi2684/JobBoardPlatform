using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;

public record CreateResumeRequestDto(
                                     [Required(ErrorMessage = "the Title is required", AllowEmptyStrings = false)]
                                     [MinLength(2, ErrorMessage = "the Title characteers cannot be lower than 2")]
                                     [MaxLength(100, ErrorMessage = "the Title characteers cannot be higher than 100")]
                                     string Title,

                                     [Required(ErrorMessage = "the UserId is required", AllowEmptyStrings = false)]
                                     Guid UserId);

