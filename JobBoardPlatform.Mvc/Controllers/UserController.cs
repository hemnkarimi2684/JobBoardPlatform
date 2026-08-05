using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize(Policy = "ActiveJobSeekerOnly")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ICityService _cityService;

    public UserController(IUserService userService, ICityService cityService)
    {
        _userService = userService;
        _cityService = cityService;
    }

    public async Task<IActionResult> Profile(CancellationToken cancellationToken = default)
    {
        var profile = await _userService.GetUserProfileByUserIdAsync(CurrentUserId(), cancellationToken);

        return View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateCitiesAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProfileRequestDto model, CancellationToken cancellationToken)
    {
        model.UserId = CurrentUserId();
        ModelState.Remove(nameof(model.UserId));

        if (!ModelState.IsValid)
        {
            await PopulateCitiesAsync(cancellationToken);
            return View(model);
        }

        try
        {
            await _userService.CreateProfileAsync(model, cancellationToken);

            TempData["Success"] = "Profile was created successfully.";

            return RedirectToAction(nameof(Profile));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateCitiesAsync(cancellationToken);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken)
    {
        var profile = await _userService.GetUserProfileByUserIdAsync(CurrentUserId(), cancellationToken);

        await PopulateCitiesAsync(cancellationToken);

        return View(new UpdateProfileRequestDto
        {
            FirstName = profile.FullName?.Split(' ', 2)[0],
            LastName = profile.FullName?.Contains(' ') == true ? profile.FullName.Split(' ', 2)[1] : null,
            Bio = profile.Bio,
            Address = profile.Address,
            BirthDate = profile.BirthDate,
            Gender = profile.Gender
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateProfileRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCitiesAsync(cancellationToken);
            return View(model);
        }

        try
        {
            await _userService.UpdateProfileAsync(CurrentUserId(), model, cancellationToken);

            TempData["Success"] = "Profile was updated successfully.";

            return RedirectToAction(nameof(Profile));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateCitiesAsync(cancellationToken);
            return View(model);
        }
    }

    public async Task<IActionResult> DownloadImage(CancellationToken cancellationToken)
    {
        try
        {
            var image = await _userService.DownloadUserImageAsync(CurrentUserId(), cancellationToken);

            return File(image.Data, image.ContentType, image.FileName);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(UploadUserImageRequestDto model, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.UploadUserImageAsync(CurrentUserId(), model, cancellationToken);

            TempData["Success"] = "Profile image was uploaded successfully.";
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(CancellationToken cancellationToken)
    {
        try
        {
            await _userService.DeleteUserImageAsync(CurrentUserId(), cancellationToken);

            TempData["Success"] = "Profile image was deleted successfully.";
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Profile));
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

    private async Task PopulateCitiesAsync(CancellationToken cancellationToken)
    {
        var cities = await _cityService.GetAllCitiesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.Cities = new SelectList(cities.Data, nameof(CityDetailResponseDto.CityId), nameof(CityDetailResponseDto.CityName));
        ViewBag.Genders = Enum.GetValues<Gender>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();
    }
}
