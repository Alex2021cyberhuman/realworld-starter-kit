using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Conduit.Shared.Validation;

public class ValidateModelAttribute : ActionFilterAttribute
{
    static ValidateModelAttribute()
    {
        Instance = new();
    }

    public static ValidateModelAttribute Instance { get; }

    public static IActionResult Executing(
        ActionContext context)
    {
        var lowerCaseSerializableError =
            new ConduitCamelCaseSerializableError(context.ModelState);
        var actionResult =
            new ObjectResult(new { errors = lowerCaseSerializableError })
            {
                StatusCode = (int)HttpStatusCode.UnprocessableEntity
            };
        return actionResult;
    }

    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid == false)
        {
            var result = Executing(context);
            await result.ExecuteResultAsync(context);
        }

        await next();
    }
}
