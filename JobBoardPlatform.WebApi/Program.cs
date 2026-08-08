using Hangfire;
using JobBoardPlatform.Application.Common.AccessClaims.UserClaim;
using JobBoardPlatform.Application.Common.Extensions;
using JobBoardPlatform.Application.Implementation.ReportJobBusiness;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using JobBoardPlatform.Infrastructure.Common.Extensions;
using JobBoardPlatform.Infrastructure.Dapper.Common.Extensions;
using JobBoardPlatform.WebApi.Middlewares;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructureDependency(builder.Configuration);
builder.Services.AddDapperDependency(builder.Configuration);
builder.Services.AddBusinessDependency(builder.Configuration);
builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

#region Add Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "🔐 Auth Edu API ",
        Version = "v1",
    });

    // اضافه کردن Bearer Token به Swagger
    var bearerScheme = new OpenApiSecurityScheme
    {
        Description = "Good Bye Gp 4",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    };
    c.AddSecurityDefinition("Bearer", bearerScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

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

var app = builder.Build();

await app.SeedDataBaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

//به ازای هر ریکوست میاد با تمام جزئیات لاگشون میکنه و نگه میداره 
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<ReportJobService>(
        "demote-expired-advertisements",
        service => service.DemoteAdvertisementsAsync(CancellationToken.None),
        Cron.Daily
    );
}
app.Run();
