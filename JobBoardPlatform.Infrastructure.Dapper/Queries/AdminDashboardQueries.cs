namespace JobBoardPlatform.Infrastructure.Dapper.Queries;

public static class AdminDashboardQueries
{
    public const string GetAdminDashboardReport= @"SELECT 
												      	COUNT(*) AS TotalUsersCount
												      FROM Users;
												      
												      SELECT
												      	COUNT(*) AS EmployersCount
												      FROM Users U
												      JOIN UserRoles UR 
												      	ON U.Id = UR.UserId
												      WHERE UR.RoleId = '5337E711-0787-4445-83DD-08DEE31DC442';
												      
												      SELECT
												      	COUNT(*) AS JobSeekersCount
												      FROM Users U
												      JOIN UserRoles UR 
												      	ON U.Id = UR.UserId
												      WHERE UR.RoleId = '42C14691-0D24-4597-83DE-08DEE31DC442';
												      
												      SELECT 
												      	COUNT(*) AS ActiveAdvertisementsCount
												      FROM Advertisements
												      	WHERE IsActive = 1;
												      
												      SELECT 
												      	COUNT(*) AS InactiveAdvertisementsCount
												      FROM Advertisements
												      	WHERE IsActive = 0;
												      
												      SELECT
												      	COUNT(*) AS EmployersCount
												      FROM Users U
												      JOIN UserRoles UR 
												      	ON U.Id = UR.UserId
												      WHERE UR.RoleId = '5337E711-0787-4445-83DD-08DEE31DC442'
												      	AND IsApproved = 0;
												      
												      SELECT 
												      	J.Status AS StatusName,
												      	COUNT(*) AS CountPerStatus
												      FROM JobApplications AS J
												      GROUP BY J.Status";
}
