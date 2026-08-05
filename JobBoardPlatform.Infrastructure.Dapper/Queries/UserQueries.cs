namespace JobBoardPlatform.Infrastructure.Dapper.Queries;

public static class UserQueries
{
    public const string GetApprovedEmployerDetail = @"
                                                      SELECT 
                                                          U.Id AS EmployerId,
                                                          U.PhoneNumber,
                                                          U.Email,
                                                          C.Id AS CompanyId,
                                                          C.Name AS CompanyName,
                                                          U.CreatedAt AS EmployerCreatedAt,
                                                          COUNT(*) OVER() AS TotalCount
                                                      FROM Users AS U
                                                      INNER JOIN Companies AS C 
                                                          ON C.OwnedByUserId = U.Id
                                                      WHERE U.IsApproved = 1
                                                        AND U.IsDeleted = 0
                                                        AND U.DeletedAt IS NULL
                                                        AND EXISTS
                                                        (
                                                            SELECT 1
                                                            FROM UserRoles AS UR
                                                            INNER JOIN Roles AS R 
                                                                ON R.Id = UR.RoleId
                                                            WHERE UR.UserId = U.Id
                                                              AND R.NormalizedName = 'EMPLOYER'
                                                        )
                                                      ORDER BY U.CreatedAt DESC, U.Id
                                                      OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

    public const string GetUnapprovedEmployerDetail = @"
                                                      SELECT 
                                                          U.Id AS EmployerId,
                                                          U.PhoneNumber,
                                                          U.Email,
                                                          C.Id AS CompanyId,
                                                          C.Name AS CompanyName,
                                                          U.CreatedAt AS EmployerCreatedAt,
                                                          COUNT(*) OVER() AS TotalCount
                                                      FROM Users AS U
                                                      INNER JOIN Companies AS C
                                                          ON C.OwnedByUserId = U.Id
                                                         AND C.IsDeleted = 0
                                                         AND C.DeletedAt IS NULL
                                                      WHERE U.IsApproved = 0
                                                        AND U.IsActive = 1
                                                        AND U.IsDeleted = 0
                                                        AND U.DeletedAt IS NULL
                                                        AND EXISTS
                                                        (
                                                            SELECT 1
                                                            FROM UserRoles AS UR
                                                            INNER JOIN Roles AS R 
                                                                ON R.Id = UR.RoleId
                                                            WHERE UR.UserId = U.Id
                                                              AND R.NormalizedName = 'EMPLOYER'
                                                        )
                                                      ORDER BY U.CreatedAt DESC, U.Id
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
                                               INNER JOIN Roles AS R
                                                   ON R.Id = UR.RoleId
                                              WHERE R.NormalizedName = 'JOBSEEKER'
                                                AND U.IsDeleted = 0
                                                AND U.DeletedAt IS NULL
                                              ORDER BY U.CreatedAt DESC, U.Id
                                              OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";
}
