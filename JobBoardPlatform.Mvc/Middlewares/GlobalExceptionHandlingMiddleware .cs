using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JobBoardPlatform.Mvc.Middlewares;

public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private const string ErrorTempDataKey = "Error";

    private static class Routes
    {
        public const string Error = "/Home/Error";
        public const string NotFound = "/Home/NotFoundPage";
        public const string AccessDenied = "/Home/AccessDenied";
        public const string Login = "/Account/Login";
    }

    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly ITempDataDictionaryFactory _tempDataFactory;

    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        ITempDataDictionaryFactory tempDataFactory)
    {
        _logger = logger;
        _tempDataFactory = tempDataFactory;
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
                _logger.LogError(exception, "The response has already started and the exception cannot be handled safely.");
                throw;
            }

            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var result = CreateHandlingResult(exception, context);

        LogException(exception, result);
        PrepareResponse(context, result.StatusCode);

        var tempData = _tempDataFactory.GetTempData(context);
        tempData[ErrorTempDataKey] = result.Message;
        tempData["StatusCode"] = result.StatusCode;
        tempData.Save();

        context.Response.Redirect(result.RedirectPath);
        return Task.CompletedTask;
    }

    private static ExceptionHandlingResult CreateHandlingResult(Exception exception, HttpContext context)
    {
        return exception switch
        {
            NotFoundException ex => new ExceptionHandlingResult(
                StatusCodes.Status404NotFound,
                GetBackRedirectPath(context, Routes.NotFound),
                GetMessageOrDefault(ex.Message, "The requested resource was not found.")),

            ForbiddenException ex => new ExceptionHandlingResult(
                StatusCodes.Status403Forbidden,
                GetBackRedirectPath(context, Routes.AccessDenied),
                GetMessageOrDefault(ex.Message, "You do not have permission to access this resource.")),

            UnauthorizedException ex when context.User.Identity?.IsAuthenticated == false => new ExceptionHandlingResult(
                StatusCodes.Status401Unauthorized,
                Routes.Login,
                GetMessageOrDefault(ex.Message, "Please log in to continue.")),

            UnauthorizedException ex => new ExceptionHandlingResult(
                StatusCodes.Status401Unauthorized,
                GetBackRedirectPath(context, Routes.AccessDenied),
                GetMessageOrDefault(ex.Message, "You do not have permission to access this resource.")),

            ConflictException ex => new ExceptionHandlingResult(
                StatusCodes.Status409Conflict,
                GetBackRedirectPath(context, Routes.Error),
                GetMessageOrDefault(ex.Message, "A conflict occurred while processing your request.")),

            ValidationException ex => new ExceptionHandlingResult(
                StatusCodes.Status400BadRequest,
                GetBackRedirectPath(context, Routes.Error),
                GetMessageOrDefault(ex.Message, "The submitted data is invalid.")),

            DomainException ex => new ExceptionHandlingResult(
                StatusCodes.Status400BadRequest,
                GetBackRedirectPath(context, Routes.Error),
                GetMessageOrDefault(ex.Message, "A business rule violation occurred.")),

            EmailSendingException ex => new ExceptionHandlingResult(
                StatusCodes.Status500InternalServerError,
                GetBackRedirectPath(context, Routes.Error),
                GetMessageOrDefault(ex.Message, "An error occurred while sending email.")),

            BadHttpRequestException ex => new ExceptionHandlingResult(
                StatusCodes.Status400BadRequest,
                GetBackRedirectPath(context, Routes.Error),
                GetMessageOrDefault(ex.Message, "The request is invalid.")),

            OperationCanceledException => new ExceptionHandlingResult(
                499,
                GetBackRedirectPath(context, Routes.Error),
                "The request was canceled."),

            _ => new ExceptionHandlingResult(
                StatusCodes.Status500InternalServerError,
                GetBackRedirectPath(context, Routes.Error),
                "Something went wrong. Please contact the administrator.")
        };
    }

    private void LogException(Exception exception, ExceptionHandlingResult result)
    {
        if (result.StatusCode >= 500)
        {
            _logger.LogError(exception, result.Message);
            return;
        }

        if (result.StatusCode == 499)
        {
            _logger.LogInformation(exception, result.Message);
            return;
        }

        _logger.LogWarning(exception, result.Message);
    }

    private static void PrepareResponse(HttpContext context, int statusCode)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
        context.Response.Headers.Pragma = "no-cache";
        context.Response.Headers.Expires = "0";
    }

    private static string GetBackRedirectPath(HttpContext context, string fallbackRoute)
    {
        var referer = context.Request.Headers.Referer.ToString();

        if (string.IsNullOrWhiteSpace(referer))
            return fallbackRoute;

        var refererUri = Uri.TryCreate(referer, UriKind.Absolute, out var uri) ? uri : null;

        if (refererUri == null ||
            !string.Equals(context.Request.Scheme + "://" + context.Request.Host.Value, refererUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            return fallbackRoute;

        return refererUri.AbsolutePath + refererUri.Query;
    }

    private static string GetMessageOrDefault(string? message, string defaultMessage)
    {
        return string.IsNullOrWhiteSpace(message) ? defaultMessage : message;
    }

    private sealed record ExceptionHandlingResult(
        int StatusCode,
        string RedirectPath,
        string Message);
}
