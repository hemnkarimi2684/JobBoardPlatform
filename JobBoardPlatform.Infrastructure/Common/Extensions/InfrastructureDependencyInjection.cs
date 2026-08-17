using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.RoleEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.Infrastructure.Common.Extensions;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options
             .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<User, Role>(options =>
         {
             options.Password.RequiredLength = 8;
             options.Password.RequireNonAlphanumeric = false;
             options.Password.RequireUppercase = true;
             options.Password.RequireLowercase = true;
             options.Password.RequireDigit = true;

             options.Lockout.MaxFailedAccessAttempts = 5;
             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
         })
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
