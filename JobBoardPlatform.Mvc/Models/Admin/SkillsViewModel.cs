using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class SkillsViewModel : Pagination<SkillDetailResponseDto>
{
    public static SkillsViewModel FromResponseDto(Pagination<SkillDetailResponseDto> source)
    {
        if (source == null)
        {
            return new SkillsViewModel
            {
                Data = new List<SkillDetailResponseDto>(),
                PageNumber = 1,
                PageSize = 10,
                TotalPageCount = 0
            };
        }

        return new SkillsViewModel
        {
            Data = source.Data ?? new List<SkillDetailResponseDto>(),
            PageNumber = source.PageNumber <= 0 ? 1 : source.PageNumber,
            PageSize = source.PageSize <= 0 ? 10 : source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
    }
}