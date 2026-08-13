using Hangfire;
using JobBoardPlatform.Application.Common.AccessClaims.UserClaim;
using JobBoardPlatform.Application.Common.Extensions;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using JobBoardPlatform.Infrastructure.Common.Extensions;
using JobBoardPlatform.Infrastructure.Dapper.Common.Extensions;
using JobBoardPlatform.Mvc.Filters;
using JobBoardPlatform.Mvc.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddInfrastructureDependency(builder.Configuration);
builder.Services.AddDapperDependency(builder.Configuration);
builder.Services.AddBusinessDependency(builder.Configuration);
builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

//اینم صرفا میره تنظیمات توی اپ ستینگ برای سری لاگ رو میخونه
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApprovedEmployerOnly", policy =>
        policy.RequireRole(RoleConstants.EmployerRoleName)
              .RequireClaim(UserClaims.EmployerClaimType, UserClaims.IsApprovedClaimValue));

    options.AddPolicy("ActiveJobSeekerOnly", policy =>
        policy.RequireRole(RoleConstants.JobSeekerRoleName)
              .RequireClaim(UserClaims.JobSeekerClaimType, UserClaims.IsActiveClaimValue));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Events.OnRedirectToAccessDenied = context =>
    {
        var returnUrl = context.HttpContext.Request.Headers.Referer.ToString();

        if (string.IsNullOrWhiteSpace(returnUrl))
            returnUrl = context.RedirectUri;

        var tempData = context.HttpContext.RequestServices
            .GetRequiredService<ITempDataDictionaryFactory>()
            .GetTempData(context.HttpContext);

        tempData["Error"] = "You do not have permission to access this page. Please log in with an approved account.";
        tempData["StatusCode"] = StatusCodes.Status403Forbidden;
        tempData.Save();

        context.Response.Redirect(returnUrl);
        return Task.CompletedTask;
    };
});

var app = builder.Build();

await app.SeedDataBaseAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//به ازای هر ریکوست میاد با تمام جزئیات لاگشون میکنه و نگه میداره 
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
