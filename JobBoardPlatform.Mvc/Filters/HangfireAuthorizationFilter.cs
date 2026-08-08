using Hangfire.Dashboard;

namespace JobBoardPlatform.Mvc.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // 1. بررسی اینکه آیا کاربر احراز هویت شده است؟
        if (httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
            return false;

        return httpContext.User.IsInRole("Admin");
    }
}
