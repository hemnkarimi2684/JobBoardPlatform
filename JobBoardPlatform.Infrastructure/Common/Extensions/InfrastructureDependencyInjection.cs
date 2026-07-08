using JobBoardPlatform.Infrastructure.Data;
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

        return services;
    }
}
