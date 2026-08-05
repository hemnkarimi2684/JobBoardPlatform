using JobBoardPlatform.Application.Common.AccessClaims.UserClaim;
using JobBoardPlatform.Application.Common.Extensions;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using JobBoardPlatform.Infrastructure.Common.Extensions;
using JobBoardPlatform.Infrastructure.Dapper.Common.Extensions;
using JobBoardPlatform.Mvc.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddInfrastructureDependency(builder.Configuration);
builder.Services.AddDapperDependency(builder.Configuration);
builder.Services.AddBusinessDependency(builder.Configuration);
builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();

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
    options.AccessDeniedPath = "/Home/Error";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
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

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
