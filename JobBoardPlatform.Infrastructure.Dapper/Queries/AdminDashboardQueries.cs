namespace JobBoardPlatform.Infrastructure.Dapper.Queries;

public static class AdminDashboardQueries
{
    public const string GetAdminDashboardReport = @"SELECT 
													COUNT(*) AS TotalUsersCount
												   FROM Users U
												   WHERE U.IsDeleted = 0 AND U.DeletedAt IS NULL;
												   
												   SELECT 
												   	COUNT(*) AS EmployersCount
												   FROM Users U
												   JOIN UserRoles UR ON U.Id = UR.UserId
												   WHERE UR.RoleId = '5337E711-0787-4445-83DD-08DEE31DC442'
												     AND U.IsActive = 1
												     AND U.IsDeleted = 0
												     AND U.DeletedAt IS NULL;
												   
												   SELECT 
												   	COUNT(*) AS JobSeekersCount
												   FROM Users U
												   JOIN UserRoles UR ON U.Id = UR.UserId
												   WHERE UR.RoleId = '42C14691-0D24-4597-83DE-08DEE31DC442'
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
												   
												   SELECT
												   	COUNT(*) AS NotApprovedEmployersCount
												   FROM Users U
												   JOIN UserRoles UR ON U.Id = UR.UserId
												   WHERE UR.RoleId = '5337E711-0787-4445-83DD-08DEE31DC442'
												     AND U.IsApproved = 0
												     AND U.IsDeleted = 0
												     AND U.DeletedAt IS NULL;
												   
												   SELECT 
												       J.Status AS StatusName,
												       COUNT(*) AS CountPerStatus
												   FROM JobApplications AS J
												   WHERE J.IsDeleted = 0
												     AND J.DeletedAt IS NULL
												   GROUP BY J.Status;";
}
