using JobBoardPlatform.Application.Common.Extensions;
using JobBoardPlatform.Infrastructure.Common.Extensions;
using JobBoardPlatform.WebApi.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructureDependency(builder.Configuration);
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

var app = builder.Build();

await app.SeedDataBaseAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
