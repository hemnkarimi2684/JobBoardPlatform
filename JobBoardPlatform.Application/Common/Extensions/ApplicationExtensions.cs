using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Implementation.AdvertisementBusiness;
using JobBoardPlatform.Application.Implementation.AttachmentBusiness;
using JobBoardPlatform.Application.Implementation.AuthenticationBusiness;
using JobBoardPlatform.Application.Implementation.CityBusiness;
using JobBoardPlatform.Application.Implementation.CompanyBusiness;
using JobBoardPlatform.Application.Implementation.EducationDetailBusiness;
using JobBoardPlatform.Application.Implementation.ExperienceDetailBusiness;
using JobBoardPlatform.Application.Implementation.JobApplicationBusiness;
using JobBoardPlatform.Application.Implementation.JobBusiness;
using JobBoardPlatform.Application.Implementation.JwtBusiness;
using JobBoardPlatform.Application.Implementation.PaymentBusiness;
using JobBoardPlatform.Application.Implementation.ProvinceBusiness;
using JobBoardPlatform.Application.Implementation.ResumeBusiness;
using JobBoardPlatform.Application.Implementation.SkillBusiness;
using JobBoardPlatform.Application.Implementation.UserBusiness;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Application.Interfaces.AuthenticationInterface;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.Application.Interfaces.JwtInterface;
using JobBoardPlatform.Application.Interfaces.PaymentInterface;
using JobBoardPlatform.Application.Interfaces.ProvinceInterface;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
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
        if (roleManager.Roles.Any()) return;

        var adminRole = new Role(RoleConstants.AdminRoleName);
        var employerRole = new Role(RoleConstants.EmployerRoleName);
        var jobSeekerRole = new Role(RoleConstants.JobSeekerRoleName);

        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(employerRole);
        await roleManager.CreateAsync(jobSeekerRole);
    }

    private static async Task SeedAdminsAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var adminsData = configuration.GetSection("AdminData").Get<List<AdminData>>();

        if (!adminsData?.Any() ?? true) return;

        var adminData = adminsData.FirstOrDefault(d => d.Role == RoleConstants.AdminRoleName);

        if (adminData != null)
        {
            var adminUser = new User(adminData.Email, adminData.PhoneNumber, true);
            await userManager.CreateAsync(adminUser, adminData.Password);
            await userManager.AddToRoleAsync(adminUser, RoleConstants.AdminRoleName);
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
        services.AddScoped<ICurrentUser, CurrentUser.Implementation.CurrentUser>();

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
