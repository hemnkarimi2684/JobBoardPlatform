using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using AccountAuthenticationService = JobBoardPlatform.Application.Interfaces.AuthenticationInterface.IAuthenticationService;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Mvc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoardPlatform.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly AccountAuthenticationService _authenticationService;
    private readonly ICityService _cityService;
    private readonly IJobCategoryService _jobCategoryService;
    private readonly IOptions<JwtSettings> _jwtSettings;

    public AccountController(
        AccountAuthenticationService authenticationService,
        ICityService cityService,
        IJobCategoryService jobCategoryService,
        IOptions<JwtSettings> jwtSettings)
    {
        _authenticationService = authenticationService;
        _cityService = cityService;
        _jobCategoryService = jobCategoryService;
        _jwtSettings = jwtSettings;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _authenticationService.LoginByEmailOrPhoneNumberAndPassword(
                new LoginRequestDto
                {
                    EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                    Password = model.Password
                },
                cancellationToken);

            await SignInWithTokenAsync(result, model.RememberMe);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult RegisterJobSeeker()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterJobSeeker(RegisterJobSeekerRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _authenticationService.RegisterJobSeekerAsync(model, cancellationToken);

            await SignInWithTokenAsync(result, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RegisterEmployer(CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        await PopulateSelectListsAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterEmployer(RegisterEmployerRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }

        try
        {
            await _authenticationService.RegisterEmployerAsync(model, cancellationToken);

            TempData["Success"] = "Your registration was completed successfully. You can sign in after admin approval.";

            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = User.FindFirst("refresh_token")?.Value;

        if (!string.IsNullOrEmpty(refreshToken))
        {
            try
            {
                await _authenticationService.LogoutAsync(
                    new LogoutRequestDto { RefreshToken = refreshToken },
                    cancellationToken);
            }
            catch (Exception)
            {
            }
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    private async Task SignInWithTokenAsync(TokenLoginResponseDto tokenResult, bool isPersistent)
    {
        var principal = ReadPrincipalFromToken(tokenResult.AccessToken)
                        ?? new ClaimsPrincipal(new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme));

        var claims = principal.Claims.ToList();
        claims.Add(new Claim("refresh_token", tokenResult.RefreshToken));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });
    }

    private ClaimsPrincipal? ReadPrincipalFromToken(string accessToken)
    {
        var settings = _jwtSettings.Value;

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            ValidAudience = settings.Audience,
            ValidateAudience = true,
            ValidIssuer = settings.Issuer,
            ValidateIssuer = true,
            ValidateLifetime = false,
            TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.EncryptKey))
        };

        try
        {
            return new JwtSecurityTokenHandler().ValidateToken(accessToken, validationParameters, out _);
        }
        catch
        {
            return null;
        }
    }

    private async Task PopulateSelectListsAsync(CancellationToken cancellationToken)
    {
        var cities = await _cityService.GetAllCitiesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.Cities = new SelectList(cities.Data, nameof(CityDetailResponseDto.CityId), nameof(CityDetailResponseDto.CityName));

        var jobCategories = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.JobCategories = new SelectList(jobCategories.Data, nameof(JobCategoryResponseDto.JobCategoryId), nameof(JobCategoryResponseDto.Name));

        ViewBag.OwnershipTypes = Enum.GetValues<OwnershipType>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();

        ViewBag.CompanySizes = Enum.GetValues<CompanySizeEnum>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();
    }
}
