using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Interfaces.ProvinceInterface;
using JobBoardPlatform.Mvc.Models.Province;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.Mvc.Controllers;

public class ProvinceController : Controller
{
    private readonly IProvinceService _provinceService;

    public ProvinceController(IProvinceService provinceService)
    {
        _provinceService = provinceService;
    }

    public async Task<IActionResult> Index(string text, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _provinceService.GetAllProvincesAsync(
            new TextRequestDto { Text = text },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 30 },
            cancellationToken);

        ViewBag.Text = text;

        return View(ProvinceIndexViewModel.FromResponseDto(result));
    }
}
