namespace JobBoardPlatform.Infrastructure.Dapper.Queries;

public static class AdminDashboardQueries
{
    public const string GetAdminDashboardReport = @"
                                       SELECT 
                                           COUNT(*) AS TotalUsersCount
                                       FROM Users U
                                       WHERE U.IsDeleted = 0 AND U.DeletedAt IS NULL;
                                       
                                       SELECT 
                                           COUNT(*) AS EmployersCount
                                       FROM Users U
                                       JOIN UserRoles UR ON U.Id = UR.UserId
                                       JOIN Roles R ON R.Id = UR.RoleId
                                       WHERE R.NormalizedName = 'EMPLOYER'
                                         AND U.IsActive = 1
                                         AND U.IsDeleted = 0
                                         AND U.DeletedAt IS NULL;
                                       
                                       SELECT 
                                           COUNT(*) AS JobSeekersCount
                                       FROM Users U
                                       JOIN UserRoles UR ON U.Id = UR.UserId
                                       JOIN Roles R ON R.Id = UR.RoleId
                                       WHERE R.NormalizedName = 'JOBSEEKER'
                                         AND U.IsActive = 1
                                         AND U.IsDeleted = 0
                                         AND U.DeletedAt IS NULL;
                                       
                                       SELECT 
                                           COUNT(*) AS ActiveAdvertisementsCount
                                       FROM Advertisements A
                                       WHERE A.IsActive = 1
                                         AND A.IsDeleted = 0
                                         AND A.DeletedAt IS NULL;
                                       
                                       SELECT
                                           COUNT(*) AS InactiveAdvertisementsCount
                                       FROM Advertisements A
                                       WHERE A.IsActive = 0
                                         AND A.IsDeleted = 0
                                         AND A.DeletedAt IS NULL;
                                       
                                       SELECT COUNT(*) AS NotApprovedEmployersCount
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
                                         );
                                       
                                       SELECT 
                                           J.Status AS StatusName,
                                           COUNT(*) AS CountPerStatus
                                       FROM JobApplications AS J
                                       WHERE J.IsDeleted = 0
                                         AND J.DeletedAt IS NULL
                                       GROUP BY J.Status;";
}
