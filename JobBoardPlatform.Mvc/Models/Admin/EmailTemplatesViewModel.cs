using JobBoardPlatform.Application.Common.Dto.ResponseDto.EmailTemplateDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class EmailTemplatesViewModel : Pagination<EmailTemplateResponseDto>
{
    public static EmailTemplatesViewModel FromResponseDto(Pagination<EmailTemplateResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
