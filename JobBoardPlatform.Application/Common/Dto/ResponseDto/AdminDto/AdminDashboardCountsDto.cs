using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;

public class AdminDashboardCountsDto
{
    //تعداد کاربران 
    public int TotalUsersCount { get; set; }
    public int JobSeekersCount { get; set; }
    public int EmployersCount { get; set; }

    //تعداد اگهی های فعال و غیرفعال
    public int ActiveAdvertisementsCount { get; set; }
    public int InactiveAdvertisementsCount { get; set; }

    // تعداد کارفرما های در انتظار تایید
    public int PendingEmployersCount { get; set; }

    //تعداد درخواست های کاری براساس وضعیت 
    public List<JobApplicationStatusCount> JobApplicationStatusCounts { get; set; } = new();
}

public class JobApplicationStatusCount
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public JobApplicationStatus StatusName { get; set; }

    public int CountPerStatus { get; set; }
}
