using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;
using JobBoardPlatform.Application.Interfaces.AdminDashboardInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.Dashboard;

[Route("api/admin/dashboards")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminDashboardsController : ControllerBase
{
    private readonly IAdminDashboardService _adminDashboardService;

    public AdminDashboardsController(IAdminDashboardService adminDashboardService)
    {
        _adminDashboardService = adminDashboardService;
    }

    [HttpGet("counts")]
    public async Task<IActionResult> AdminDashboardReportAsync()
    {
        var result = await _adminDashboardService.GetCountsAsync();

        return Ok(Result<AdminDashboardCountsDto>.Success(result));
    }
}
