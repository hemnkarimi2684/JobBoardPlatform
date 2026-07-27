using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Infrastructure.Dapper.Connection;
using JobBoardPlatform.Infrastructure.Dapper.Repositories.UserRepo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.Infrastructure.Dapper.Common.Extensions;

public static class DapperDependencyInjection
{
    public static IServiceCollection AddDapperDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(serviceProvider => new DbConnectionFactory(configuration.GetConnectionString("DefaultConnection")!)); ;

        services.AddScoped<IUserDapperRepository, UserDapperRepository>();

        return services;
    }
}
