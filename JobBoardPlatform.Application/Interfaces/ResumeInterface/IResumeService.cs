using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;

namespace JobBoardPlatform.Application.Interfaces.ResumeInterface;

public interface IResumeService
{
    /// <summary>
    /// ساخت رزومه برای کاربر 
    /// </summary>
    /// <param name="resumeCommand"></param>
    /// <returns></returns>
    Task<bool> CreateResumeAsync(CreateResumeRequestDto resumeCommand);

    /// <summary>
    /// دریافت رزومه با شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResumeDetailResponseDto> GetResumeByUserIdAsync(Guid userId);
}
