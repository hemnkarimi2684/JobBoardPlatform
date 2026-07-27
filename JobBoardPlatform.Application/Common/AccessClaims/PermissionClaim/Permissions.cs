namespace JobBoardPlatform.Application.Common.AccessClaims.PermissionClaim;

public static class Permissions
{
    public static class Advertisements
    {
        public const string Read = "Advertisements.Read";
        public const string Delete = "Advertisements.Delete";
        public const string Activate = "Advertisements.Activate";
        public const string Feature = "Advertisements.Feature";
    }

    public static class Users
    {
        public const string Read = "Users.Read";
        public const string Activate = "Users.Activate";
    }

    public static class UserProfiles
    {
        public const string Read = "UserProfiles.Read";
    }

    public static class Employers
    {
        public const string Read = "Employers.Read";
        public const string Approve = "Employers.Approve";
    }

    public static class EmployerProfiles
    {
        public const string Read = "EmployerProfiles.Read";
    }
}
