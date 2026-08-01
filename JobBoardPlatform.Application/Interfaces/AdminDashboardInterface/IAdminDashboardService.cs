using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;

namespace JobBoardPlatform.Application.Interfaces.AdminDashboardInterface;

public interface IAdminDashboardService
{
    /// <summary>
    /// دریافت گزارش از دشبورد ادمین
    /// </summary>
    /// <returns></returns>
    Task<AdminDashboardCountsDto> GetCountsAsync();
}
