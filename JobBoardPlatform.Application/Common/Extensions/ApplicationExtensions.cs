using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.EmailSettings;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Implementation.AccessControlBusiness;
using JobBoardPlatform.Application.Implementation.AdminDashboardBusiness;
using JobBoardPlatform.Application.Implementation.AdvertisementBusiness;
using JobBoardPlatform.Application.Implementation.AttachmentBusiness;
using JobBoardPlatform.Application.Implementation.AuthenticationBusiness;
using JobBoardPlatform.Application.Implementation.CityBusiness;
using JobBoardPlatform.Application.Implementation.CompanyBusiness;
using JobBoardPlatform.Application.Implementation.EducationDetailBusiness;
using JobBoardPlatform.Application.Implementation.EmailBusiness;
using JobBoardPlatform.Application.Implementation.ExperienceDetailBusiness;
using JobBoardPlatform.Application.Implementation.JobApplicationBusiness;
using JobBoardPlatform.Application.Implementation.JobBusiness;
using JobBoardPlatform.Application.Implementation.JobCategoryBusiness;
using JobBoardPlatform.Application.Implementation.JwtBusiness;
using JobBoardPlatform.Application.Implementation.PaymentBusiness;
using JobBoardPlatform.Application.Implementation.ProvinceBusiness;
using JobBoardPlatform.Application.Implementation.RedisBusiness;
using JobBoardPlatform.Application.Implementation.RefreshTokenBusiness;
using JobBoardPlatform.Application.Implementation.ReportJobBusiness;
using JobBoardPlatform.Application.Implementation.ResumeBusiness;
using JobBoardPlatform.Application.Implementation.SkillBusiness;
using JobBoardPlatform.Application.Implementation.UserBusiness;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AdminDashboardInterface;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Application.Interfaces.AuthenticationInterface;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.Application.Interfaces.JwtInterface;
using JobBoardPlatform.Application.Interfaces.PaymentInterface;
using JobBoardPlatform.Application.Interfaces.ProvinceInterface;
using JobBoardPlatform.Application.Interfaces.RedisInterface;
using JobBoardPlatform.Application.Interfaces.RefreshTokenInterface;
using JobBoardPlatform.Application.Interfaces.ReportJobBusiness;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using JobBoardPlatform.Core.Entities.RoleEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace JobBoardPlatform.Application.Common.Extensions;

public static class ApplicationExtensions
{
    public static async Task SeedDataBaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        await SeedRolesAsync(scope.ServiceProvider);
        await SeedAdminsAsync(scope.ServiceProvider);
    }

    private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        var rolesToSeed = new (string Name, string Description)[]
        {
            (RoleConstants.AdminRoleName, "A system administrator who manages users, jobs, and platform settings."),
            (RoleConstants.EmployerRoleName, "A company representative who creates job postings and reviews applicants."),
            (RoleConstants.JobSeekerRoleName, "A user who searches and applies for jobs.")
        };

        foreach (var (name, description) in rolesToSeed)
        {
            if (await roleManager.FindByNameAsync(name) != null)
                continue;

            var existingRole = roleManager.Roles.FirstOrDefault(r => r.Name == name);

            if (existingRole != null)
            {
                existingRole.NormalizedName = roleManager.NormalizeKey(name);
                await roleManager.UpdateAsync(existingRole);
                continue;
            }

            await roleManager.CreateAsync(new Role(name, description));
        }
    }

    private static async Task SeedAdminsAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var adminData = configuration.GetSection("AdminData").Get<AdminData>();

        if (adminData == null)
            return;

        var adminUser = await userManager.FindByEmailAsync(adminData.Email);

        if (adminUser == null)
        {
            adminUser = new User(adminData.Email, adminData.PhoneNumber, true);
            var createUserResult = await userManager.CreateAsync(adminUser, adminData.Password);

            if (!createUserResult.Succeeded)
                throw new ValidationException(string.Join(" ", createUserResult.Errors.Select(e => e.Description)));
        }

        var isInRole = await userManager.IsInRoleAsync(adminUser, RoleConstants.AdminRoleName);

        if (!isInRole)
        {
            var addUserToRoleResult = await userManager.AddToRoleAsync(adminUser, RoleConstants.AdminRoleName);

            if (!addUserToRoleResult.Succeeded)
                throw new ValidationException(string.Join(" ", addUserToRoleResult.Errors.Select(e => e.Description)));
        }
    }

    public static IServiceCollection AddBusinessDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAdvertisementService, AdvertisementService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IEducationDetailService, EducationDetailService>();
        services.AddScoped<IExperienceDetailService, ExperienceDetailService>();
        services.AddScoped<IJobApplicationService, JobApplicationService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IProvinceService, ProvinceService>();
        services.AddScoped<IResumeService, ResumeService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IReportJobService, ReportJobService>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser.Implementation.CurrentUser>();

        services.AddScoped<IAccessControlService, AccessControlService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IJobCategoryService, JobCategoryService>();
        services.AddScoped<IRedisService, RedisService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();

        services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
        services.AddScoped<IEmailService, EmailService>();


        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "JobBoardPlatform:";
        });

        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Secret)),
                ValidAudience = jwtSettings.Audience,
                ValidateAudience = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateIssuer = true,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.EncryptKey))
            };
        });

        return services;
    }
}
