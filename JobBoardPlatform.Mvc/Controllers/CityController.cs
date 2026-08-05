using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Mvc.Models.City;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.Mvc.Controllers;

public class CityController : Controller
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<IActionResult> Index(string text, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _cityService.GetAllCitiesAsync(
            new TextRequestDto { Text = text },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        ViewBag.Text = text;

        return View(CityIndexViewModel.FromResponseDto(result));
    }

    public async Task<IActionResult> Details(Guid id, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _cityService.GetCityCompaniesAsync(
            id,
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        return View(CityDetailsViewModel.FromResponseDto(result));
    }
}
