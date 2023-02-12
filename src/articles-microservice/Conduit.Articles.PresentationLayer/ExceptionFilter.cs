using System.Net;
using Conduit.Articles.DomainLayer.Exceptions;
using Conduit.Shared.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Conduit.Articles.PresentationLayer;

public class ExceptionFilter : IMiddleware
{
    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        var logger = context.RequestServices
            .GetRequiredService<ILogger<ExceptionFilter>>();
        try
        {
            await next(context);
        }
        catch (ForbiddenException exception)
        {
            logger.LogWarning(exception, "Catch ForbiddenException while processing request");
            var result = new ForbidResult();
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
        catch (InvalidRequestException exception)
        {
            logger.LogWarning(exception, "Catch InvalidRequestException while processing request");
            var result = new ObjectResult(new ConduitCamelCaseSerializableError(GetModelStateDictionary(exception)))
            {
                StatusCode = (int)HttpStatusCode.UnprocessableEntity
            };
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
        catch (BadRequestException exception)
        {
            logger.LogWarning(exception, "Catch BadRequestException while processing request");
            var result = new BadRequestResult();
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
        catch (NotFoundException exception)
        {
            logger.LogWarning(exception, "Catch NotFoundException while processing request");
            var result = new NotFoundResult();
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
    }

    private static ModelStateDictionary GetModelStateDictionary(
        InvalidRequestException exception)
    {
        var modelState = new ModelStateDictionary();

        foreach (var validationResult in exception.ValidationResults)
        {
            if (!string.IsNullOrWhiteSpace(validationResult.ErrorMessage))
            {
                modelState.AddModelError(validationResult.MemberNames.First(),
                    validationResult.ErrorMessage);
            }
        }

        return modelState;
    }
}
