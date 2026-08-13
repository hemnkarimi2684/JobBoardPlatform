using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using JobBoardPlatform.Mvc.Models.User;
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
        UserProfileViewModel model;

        try
        {
            var profile = await _userService.GetUserProfileByUserIdAsync(CurrentUserId(), cancellationToken);
            model = UserProfileViewModel.FromResponseDto(profile);
        }
        catch (NotFoundException)
        {
            model = new UserProfileViewModel();
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        await PopulateCitiesAsync(cancellationToken);
        return View(new UserCreateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateViewModel model, CancellationToken cancellationToken = default)
    {
        model.UserId = CurrentUserId();
        ModelState.Remove(nameof(model.UserId));

        if (!ModelState.IsValid)
        {
            await PopulateCitiesAsync(cancellationToken);
            return View(model);
        }

        await _userService.CreateProfileAsync(model, cancellationToken);

        TempData["Success"] = "Profile was created successfully.";

        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken = default)
    {
        var profile = await _userService.GetUserProfileByUserIdAsync(CurrentUserId(), cancellationToken);

        await PopulateCitiesAsync(cancellationToken);

        return View(UserEditViewModel.FromResponseDto(profile));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserEditViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCitiesAsync(cancellationToken);
            return View(model);
        }

        await _userService.UpdateProfileAsync(CurrentUserId(), model, cancellationToken);

        TempData["Success"] = "Profile was updated successfully.";

        return RedirectToAction(nameof(Profile));
    }

    public async Task<IActionResult> DownloadImage(CancellationToken cancellationToken = default)
    {
        var image = await _userService.DownloadUserImageAsync(CurrentUserId(), cancellationToken);

        return File(image.Data, image.ContentType, image.FileName);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(UploadUserImageRequestDto model, CancellationToken cancellationToken = default)
    {
        await _userService.UploadUserImageAsync(CurrentUserId(), model, cancellationToken);

        TempData["Success"] = "Profile image was uploaded successfully.";

        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(CancellationToken cancellationToken = default)
    {
        await _userService.DeleteUserImageAsync(CurrentUserId(), cancellationToken);

        TempData["Success"] = "Profile image was deleted successfully.";

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

        ViewBag.Cities = new SelectList(
            cities.Data,
            nameof(CityDetailResponseDto.CityId),
            nameof(CityDetailResponseDto.CityName));

        ViewBag.Genders = Enum.GetValues<Gender>()
            .Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.ToString()
            })
            .ToList();
    }
}