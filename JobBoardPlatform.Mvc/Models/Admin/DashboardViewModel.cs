using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class DashboardViewModel : AdminDashboardCountsDto
{
    public static DashboardViewModel FromResponseDto(AdminDashboardCountsDto source)
        => new()
        {
            TotalUsersCount = source.TotalUsersCount,
            JobSeekersCount = source.JobSeekersCount,
            EmployersCount = source.EmployersCount,
            ActiveAdvertisementsCount = source.ActiveAdvertisementsCount,
            InactiveAdvertisementsCount = source.InactiveAdvertisementsCount,
            PendingEmployersCount = source.PendingEmployersCount,
            JobApplicationStatusCounts = source.JobApplicationStatusCounts
        };
}
