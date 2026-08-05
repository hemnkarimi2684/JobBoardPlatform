using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class SkillsViewModel : Pagination<SkillDetailResponseDto>
{
    public static SkillsViewModel FromResponseDto(Pagination<SkillDetailResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
