using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Skill;

public class SkillMySkillsViewModel : Pagination<UserSkillResponseDto>
{
    public List<SkillDetailResponseDto> AvailableSkills { get; set; } = new();

    public static SkillMySkillsViewModel FromResponseDto(
        Pagination<UserSkillResponseDto> source,
        List<SkillDetailResponseDto> availableSkills)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount,
            AvailableSkills = availableSkills
        };
}
