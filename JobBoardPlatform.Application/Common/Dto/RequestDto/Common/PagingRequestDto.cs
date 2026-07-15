using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.Common;

public record PagingRequestDto(
                                [Range(0, 100, ErrorMessage = "the PageNumber must be in the range")]
                                int PageNumber = 1,

                                [Range(0, 1000, ErrorMessage = "the PageSize must be in the range")]
                                int PageSize = 10);

