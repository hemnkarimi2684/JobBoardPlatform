using Microsoft.Data.SqlClient;
using System.Data;

namespace JobBoardPlatform.Infrastructure.Dapper.Connection;

public interface IDbConnectionFactory
{
    /// <summary>
    /// اتصال با دیتابیس برای دپر
    /// </summary>
    /// <returns></returns>
    SqlConnection CreateConnection();
}
