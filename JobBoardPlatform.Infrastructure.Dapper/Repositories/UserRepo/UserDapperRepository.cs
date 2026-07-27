using Dapper;
using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Core.Entities.UserEntity.ReadModels;
using JobBoardPlatform.Infrastructure.Dapper.Connection;
using JobBoardPlatform.Infrastructure.Dapper.Queries;
using System.Data.Common;

namespace JobBoardPlatform.Infrastructure.Dapper.Repositories.UserRepo;

public class UserDapperRepository : IUserDapperRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserDapperRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<(IEnumerable<JobSeekerDetailReadModel> Items, int TotalDataCount)> GetJobSeekersAsync(
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        await using var connection = _connectionFactory.CreateConnection();

        var query = UserQueries.GetJobSeekerDetail;

        var param = new
        {
            Skip = (pageNumber - 1) * pageSize,
            Take = pageSize
        };

        var result = await connection.QueryAsync<JobSeekerDetailReadModel>(query, param);

        //چک کردن اینکه اگه کلا لیسته خالی بود 
        int totalCount = result.FirstOrDefault()?.TotalCount ?? 0;

        return (result, totalCount);
    }

    public async Task<(IEnumerable<EmployerDetailReadModel> Items, int TotalDataCount)> GetApprovedEmployersAsync(
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        await using var connection = _connectionFactory.CreateConnection();

        var query = UserQueries.GetApprovedEmployerDetail;

        var param = new
        {
            Skip = (pageNumber - 1) * pageSize,
            Take = pageSize
        };

        var result = await connection.QueryAsync<EmployerDetailReadModel>(query, param);

        //چک کردن اینکه اگه کلا لیسته خالی بود 
        int totalCount = result.FirstOrDefault()?.TotalCount ?? 0;

        return (result, totalCount);
    }
}
