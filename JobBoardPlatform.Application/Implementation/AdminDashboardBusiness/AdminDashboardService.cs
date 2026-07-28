using Dapper;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AdminDashboardInterface;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using JobBoardPlatform.Infrastructure.Dapper.Connection;
using JobBoardPlatform.Infrastructure.Dapper.Queries;
using Microsoft.AspNetCore.Connections;

namespace JobBoardPlatform.Application.Implementation.AdminDashboardBusiness;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IDbConnectionFactory _connection;

    private readonly IAccessControlService _accessControlService;

    private readonly ICurrentUser _currentUser;

    public AdminDashboardService(IDbConnectionFactory connection, IAccessControlService accessControlService, ICurrentUser currentUser)
    {
        _connection = connection;
        _accessControlService = accessControlService;
        _currentUser = currentUser;
    }

    public async Task<AdminDashboardCountsDto> GetCountsAsync()
    {
        _accessControlService.EnsureAdmin(_currentUser);

        await using var connection = _connection.CreateConnection();

        var query = AdminDashboardQueries.GetAdminDashboardReport;

        using var multi = await connection.QueryMultipleAsync(query);

        return new AdminDashboardCountsDto
        {
            TotalUsersCount = await multi.ReadFirstAsync<int>(),
            EmployersCount = await multi.ReadFirstAsync<int>(),
            JobSeekersCount = await multi.ReadFirstAsync<int>(),
            ActiveAdvertisementsCount = await multi.ReadFirstAsync<int>(),
            InactiveAdvertisementsCount = await multi.ReadFirstAsync<int>(),
            PendingEmployersCount = await multi.ReadFirstAsync<int>(),
            JobApplicationStatusCounts = (await multi.ReadAsync<JobApplicationStatusCount>()).ToList(),
        };
    }
}
