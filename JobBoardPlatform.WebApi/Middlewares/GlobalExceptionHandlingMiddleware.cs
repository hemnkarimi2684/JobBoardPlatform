using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;
using JobBoardPlatform.WebApi.ResultPattern;
using System.Text.Json;

namespace JobBoardPlatform.WebApi.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await ExceptionHandlerAsync(context, exception);
        }
    }

    private async Task ExceptionHandlerAsync(HttpContext context, Exception exception)
    {
        switch (exception)
        {
            case NotFoundException notFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(GenerateResponseBody(notFoundException.Message, notFoundException.Code));
                break;
            case ConflictException conflictException:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsync(GenerateResponseBody(conflictException.Message, conflictException.Code));
                break;
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(GenerateResponseBody(validationException.Message, validationException.Code));
                break;
            case DomainException domainException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(GenerateResponseBody(domainException.Message, domainException.Code));
                break;
            case ForbiddenException forbiddenException:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(GenerateResponseBody(forbiddenException.Message, forbiddenException.Code));
                break;
            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(GenerateResponseBody(unauthorizedException.Message, unauthorizedException.Code));
                break;
            case OperationCanceledException:
                context.Response.StatusCode = 499;
                await context.Response.WriteAsync(GenerateResponseBody("The request was canceled.", "request_canceled"));
                break;
            case FormatException formatEx when formatEx.Message.Contains("Base-64", StringComparison.OrdinalIgnoreCase):
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(GenerateResponseBody("Invalid username or password.", "Invalid_Credentials"));
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(GenerateResponseBody
                    ("InternalServerError_500", "Something went wrong. Please contact your administrator."));
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
