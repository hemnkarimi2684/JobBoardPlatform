using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.SkillInterface;

public interface ISkillService
{
    /// <summary>
    /// دریافت مهارت های یک کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Pagination<UserSkillDetailResponseDto>> GetUserSkillsAsync(Guid userId, PagingRequestDto pagingCommand);

    /// <summary>
    /// ساخت یک مهارت 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> CreateSkillAsync(string name);

    /// <summary>
    /// اضافه کردن مهارت به یک کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="skillsId"></param>
    /// <returns></returns>
    Task<bool> AddSkillsToUserAsync(Guid userId, List<Guid> skillsId);

    /// <summary>
    /// دریافت تمام مهارت ها 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pagingCommand"></param>
    /// <returns></returns>
    Task<Pagination<UserSkillDetailResponseDto>> GetAllSkillsAsync(string text, PagingRequestDto pagingCommand);
}
