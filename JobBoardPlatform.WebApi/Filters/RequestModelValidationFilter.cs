using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoardPlatform.WebApi.Filters;

public class RequestModelValidationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage);

            throw new ValidationException(string.Join(", ", errors));
        }
    }
}
