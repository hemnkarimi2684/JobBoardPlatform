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
                                                      WHERE UR.RoleId = '5337E711-0787-4445-83DD-08DEE31DC442' 
                                                          AND U.IsApproved = 1
                                                          AND U.IsDeleted = 0
                                                          AND U.DeletedAt IS NULL
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
                                                  AND UR.RoleId = '42C14691-0D24-4597-83DE-08DEE31DC442'
                                           	   WHERE U.IsActive = 1
                                           	  	AND U.IsDeleted = 0
                                           	  	AND U.DeletedAt IS NULL
                                               ORDER BY U.CreatedAt DESC
                                               OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";
}
