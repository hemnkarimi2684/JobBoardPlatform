using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Result;

namespace JobBoardPlatform.Application.Interfaces.ResumeInterface;

public interface IResumeService
{
    /// <summary>
    /// ساخت رزومه برای کاربر 
    /// </summary>
    /// <param name="resumeCommand"></param>
    /// <returns></returns>
    Task<bool> CreateResumeAsync(CreateResumeCommand resumeCommand);

    /// <summary>
    /// دریافت رزومه با شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResumeDetailResult> GetResumeByUserIdAsync(Guid userId);
}
