namespace JobBoardPlatform.Infrastructure.Dapper.Queries;

public static class UserQueries
{
    public const string GetApprovedEmployerDetail = @"SELECT 
                                                          U.Id AS EmployerId,
                                                          U.PhoneNumber,
                                                          U.Email,
                                                          C.Id AS CompanyId,
                                                          C.Name AS CompanyName,
                                                          U.CreatedAt AS EmployerCreatedAt,
                                                          COUNT(*) OVER() AS TotalCount
                                                      FROM Users U
                                                      INNER JOIN UserRoles UR ON U.Id = UR.UserId
                                                      INNER JOIN Companies C ON U.Id = C.OwnedByUserId
                                                      WHERE UR.RoleId = @RoleId AND U.IsApproved = 1
                                                      ORDER BY U.CreatedAt DESC
                                                      OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

    public const string GetJobSeekerDetail = @"SELECT
                                                   U.Id,
                                                   U.PhoneNumber,
                                                   U.Email,
                                                   U.IsActive,
                                                   U.CreatedAt,
                                                   COUNT(*) OVER() AS TotalCount
                                               FROM Users AS U
                                               INNER JOIN UserRoles AS UR
                                                   ON UR.UserId = U.Id
                                                  AND UR.RoleId = @RoleId
                                               ORDER BY U.CreatedAt DESC
                                               OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";
}
