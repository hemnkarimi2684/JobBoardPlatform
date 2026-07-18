using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Implementation.AdvertisementBusiness;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Advertisements;

[Route("api/[controller]s")]
[ApiController]
[Authorize]
public class AdvertisementController : ControllerBase
{
    private readonly IAdvertisementService _advertisementService;

    public AdvertisementController(IAdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpPost]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> CreateAdvertisementAsync([FromBody] CreateAdvertisementRequestDto createAdvertisement)
    {
        await _advertisementService.CreateAdvertisementAsync(createAdvertisement);

        return Ok(Result.Success());
    }

    [HttpPut("{advertisementId:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> UpdateAdvertisementAsync([FromRoute] Guid advertisementId, [FromBody] UpdateAdvertisementRequestDto updateAdvertisement)
    {
        await _advertisementService.UpdateAdvertisementAsync(advertisementId, updateAdvertisement);

        return Ok(Result.Success());
    }

    [HttpDelete("{advertisementId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDeleteAdvertisementAsync([FromRoute] Guid advertisementId)
    {
        await _advertisementService.SoftDeleteAdvertisementAsync(advertisementId);

        return Ok(Result.Success());
    }

    [HttpGet("by-company/{companyId:guid}")]
    [Authorize(Roles = "Employer,Admin")]
    public async Task<IActionResult> GetAdvertisementsByCompanyAsync([FromRoute] Guid companyId, [FromQuery] PagingRequestDto pagingRequestDto)
    {
        var result = await _advertisementService.GetAdvertisementsByCompanyAsync(pagingRequestDto, companyId);

        return Ok(Result<Pagination<AdvertisementDetailResponseDto>>.Success(result));
    }

    [HttpPatch("{advertisementId:guid}/in-activate")]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<IActionResult> InActivateAdvertisementAsync([FromRoute] Guid advertisementId)
    {
        await _advertisementService.InActivateAdvertisementAsync(advertisementId);

        return Ok(Result.Success());
    }

    [HttpPatch("{advertisementId:guid}/activate")]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<IActionResult> ActivateAdvertisementAsync([FromRoute] Guid advertisementId)
    {
        var result = await _advertisementService.ActivateAdvertisementAsync(advertisementId);

        return Ok(Result.Success());
    }

    [HttpGet("{advertisementId:guid}")]
    [Authorize(Roles = "Employer,Admin")]
    public async Task<IActionResult> GetAdvertisementInfoByIdAsync(Guid advertisementId)
    {
        var result = await _advertisementService.GetAdvertisementInfoByIdAsync(advertisementId);

        return Ok(Result<AdvertisementDetailResponseDto>.Success(result));
    }

}
