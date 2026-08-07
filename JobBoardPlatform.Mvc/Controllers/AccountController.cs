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
using JobBoardPlatform.Mvc.Models.Account;
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
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        AccountAuthenticationService authenticationService,
        ICityService cityService,
        IJobCategoryService jobCategoryService,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AccountController> logger)
    {
        _authenticationService = authenticationService;
        _cityService = cityService;
        _jobCategoryService = jobCategoryService;
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Index), "Home");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

            return RedirectToAction(nameof(Index), "Home");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult RegisterJobSeeker()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Index), "Home");

        return View(new RegisterJobSeekerViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterJobSeeker(RegisterJobSeekerViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _authenticationService.RegisterJobSeekerAsync(model, cancellationToken);

            await SignInWithTokenAsync(result, isPersistent: false);

            return RedirectToAction(nameof(Index), "Home");
        }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RegisterEmployer(CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Index), "Home");

        await PopulateSelectListsAsync(cancellationToken);
        return View(new RegisterEmployerViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterEmployer(RegisterEmployerViewModel model, CancellationToken cancellationToken)
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
        catch (AppException ex)
        {
            _logger.LogWarning(ex, "Employer registration failed.");

            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = User.FindFirst("refresh_token")?.Value;

        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            try
            {
                await _authenticationService.LogoutAsync(
                    new LogoutRequestDto { RefreshToken = refreshToken },
                    cancellationToken);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Logout service failed while revoking refresh token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during logout.");
            }
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Index), "Home");
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
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read principal from access token.");
            return null;
        }
    }

    private async Task PopulateSelectListsAsync(
    CancellationToken cancellationToken)
    {
        var cities = await _cityService.GetAllForSelectAsync(
            cancellationToken);

        var jobCategories = await _jobCategoryService.GetAllForSelectAsync(
            cancellationToken);

        ViewBag.Cities = new SelectList(
            cities,
            nameof(CityDetailResponseDto.CityId),
            nameof(CityDetailResponseDto.CityName));

        ViewBag.JobCategories = new SelectList(
            jobCategories,
            nameof(JobCategoryResponseDto.JobCategoryId),
            nameof(JobCategoryResponseDto.Name));

        ViewBag.OwnershipTypes = Enum.GetValues<OwnershipType>()
            .Select(value => new SelectListItem
            {
                Value = ((int)value).ToString(),
                Text = value.ToString()
            })
            .ToList();

        ViewBag.CompanySizes = Enum.GetValues<CompanySizeEnum>()
            .Select(value => new SelectListItem
            {
                Value = ((int)value).ToString(),
                Text = value.ToString()
            })
            .ToList();
    }
}
