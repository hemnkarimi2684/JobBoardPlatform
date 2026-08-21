using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;
using JobBoardPlatform.WebApi.ResultPattern;
using System.Text.Json;

namespace JobBoardPlatform.WebApi.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(exception, "The response has already started, cannot handle exception safely.");
                throw;
            }

            await ExceptionHandlerAsync(context, exception);
        }
    }

    private async Task ExceptionHandlerAsync(HttpContext context, Exception exception)
    {
        switch (exception)
        {
            case NotFoundException notFoundException:
                _logger.LogWarning(exception, "Resource not found: {Message}", notFoundException.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(GenerateResponseBody(notFoundException.Message, notFoundException.Code));
                break;
            case ConflictException conflictException:
                _logger.LogWarning(exception, "Conflict: {Message}", conflictException.Message);
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsync(GenerateResponseBody(conflictException.Message, conflictException.Code));
                break;
            case ValidationException validationException:
                _logger.LogWarning(exception, "Validation failed: {Message}", validationException.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(GenerateResponseBody(validationException.Message, validationException.Code));
                break;
            case DomainException domainException:
                _logger.LogWarning(exception, "Domain rule violation: {Message}", domainException.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(GenerateResponseBody(domainException.Message, domainException.Code));
                break;
            case ForbiddenException forbiddenException:
                _logger.LogWarning(exception, "Access denied: {Message}", forbiddenException.Message);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(GenerateResponseBody(forbiddenException.Message, forbiddenException.Code));
                break;
            case UnauthorizedException unauthorizedException:
                _logger.LogWarning(exception, "Unauthorized: {Message}", unauthorizedException.Message);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(GenerateResponseBody(unauthorizedException.Message, unauthorizedException.Code));
                break;
            case EmailSendingException emailSendingException:
                _logger.LogError(exception, "Email sending failed: {Message}", emailSendingException.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(GenerateResponseBody(emailSendingException.Message, emailSendingException.Code));
                break;
            case OperationCanceledException:
                _logger.LogInformation(exception, "Request was canceled");
                context.Response.StatusCode = 499;
                await context.Response.WriteAsync(GenerateResponseBody("The request was canceled.", "request_canceled"));
                break;
            case FormatException formatEx when formatEx.Message.Contains("Base-64", StringComparison.OrdinalIgnoreCase):
                _logger.LogWarning(exception, "Invalid credentials");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(GenerateResponseBody("Invalid username or password.", "Invalid_Credentials"));
                break;
            default:
                _logger.LogError(exception, "Unhandled exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(GenerateResponseBody
                    ("Something went wrong. Please contact your administrator.", "InternalServerError_500"));
                break;
        }
    }

    private string GenerateResponseBody(string message, string code)
    {
        var error = new Error(message, code);

        var result = Result.Failure(error);

        return JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
