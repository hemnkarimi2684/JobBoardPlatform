using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.SkillDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.SkillInterface;

public interface ISkillService
{
    /// <summary>
    /// دریافت مهارت های یک کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<UserSkillResponseDto>> GetUserSkillsAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ساخت یک مهارت
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CreateSkillAsync(
        CreateSkillRequestDto skillRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اضافه کردن مهارت به یک کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="skillsId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AddSkillsToUserAsync(
        Guid userId,
        List<Guid> skillsId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام مهارت ها 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<SkillDetailResponseDto>> GetAllSkillsAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت مهارت توسط شناسه اش
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SkillDetailResponseDto> GetSkillByIdAsync(
        Guid skillId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام مهارت های برای دراپ داون 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<SkillDetailResponseDto>> GetAllForSelectAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// برداشتن مهارت از یک کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="skillsId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveSkillFromUserAsync(
        Guid userId,
        List<Guid> skillsId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف نرم مهارت
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SoftDeleteAsync(
        Guid skillId,
        CancellationToken cancellationToken = default);
}
