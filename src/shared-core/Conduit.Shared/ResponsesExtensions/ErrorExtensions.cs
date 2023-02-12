using System.Net;
using Conduit.Shared.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conduit.Shared.ResponsesExtensions;

public static class ErrorExtensions
{
    public static IActionResult GetAndLogActionResult(
        this Error error,
        object? output = null,
        Validation.Validation? validation = null,
        ILogger? logger = null,
        string? errorDescription = null)
    {
        switch (error)
        {
            case Error.BadRequest:
                logger?.LogWarning("BadRequest error");
                break;
            case Error.NotFound:
                logger?.LogWarning("NotFound error");
                break;
            case Error.Forbidden:
                logger?.LogWarning("Forbidden error");
                break;
        }

        return error switch
        {
            Error.None => output != null ? new OkObjectResult(output) : errorDescription != null ? new OkObjectResult(errorDescription) : new NoContentResult(),
            Error.BadRequest => validation != null ? validation.ToBadRequest() : (errorDescription != null ? new Validation.Validation(errorDescription) : null).ToBadRequest(),
            Error.NotFound => errorDescription != null ? new NotFoundObjectResult(errorDescription) : new NotFoundResult(),
            Error.Forbidden => errorDescription != null ? new ObjectResult(errorDescription) { StatusCode = (int)HttpStatusCode.Forbidden } : new ForbidResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }

    public static IActionResult GetActionResult(
        this Error error,
        object? output,
        Validation.Validation? validation)
    {
        return error switch
        {
            Error.None => output != null ? new OkObjectResult(output) : new NoContentResult(),
            Error.BadRequest => validation.ToBadRequest(),
            Error.NotFound => new NotFoundResult(),
            Error.Forbidden => new ForbidResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}
