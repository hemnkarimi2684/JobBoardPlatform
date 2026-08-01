using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.Advertisements;

[Route("api/admin/advertisements")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminAdvertisementsController : ControllerBase
{
    private readonly IAdvertisementService _advertisementService;

    public AdminAdvertisementsController(IAdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAdvertisementsAsync(
        [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _advertisementService.GetAllAdvertisementsAsync(pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{advertisementId:guid}/activate")]
    public async Task<IActionResult> ActivateAdvertisementAsync(
        [FromRoute] Guid advertisementId,
        CancellationToken cancellationToken)
    {
        await _advertisementService.ActivateAdvertisementAsync(advertisementId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{advertisementId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateAdvertisementAsync(
        [FromRoute] Guid advertisementId,
        CancellationToken cancellationToken)
    {
        await _advertisementService.DeactivateAdvertisementAsync(advertisementId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpDelete("{advertisementId:guid}")]
    public async Task<IActionResult> SoftDeleteAdvertisementAsync(
    [FromRoute] Guid advertisementId,
    CancellationToken cancellationToken)
    {
        await _advertisementService.SoftDeleteAdvertisementAsync(advertisementId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{advertisementId:guid}/promote")]
    public async Task<IActionResult> PromoteAdvertisementAsync(
       [FromRoute] Guid advertisementId,
       [FromBody] int durationInDays,
        CancellationToken cancellationToken)
    {
        await _advertisementService.PromoteAdvertisementAsync(advertisementId, durationInDays, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{advertisementId:guid}/Demote")]
    public async Task<IActionResult> DemoteAdvertisementAsync(
       [FromRoute] Guid advertisementId,
        CancellationToken cancellationToken)
    {
        await _advertisementService.DemoteAdvertisementAsync(advertisementId, cancellationToken);

        return Ok(Result.Success());
    }
}
