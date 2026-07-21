using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.Common;

public class PagingRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than or equal to 1.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
    public int PageSize { get; set; } = 10;
}

