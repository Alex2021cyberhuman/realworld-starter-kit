using Conduit.Shared.Outcomes;
using Conduit.Shared.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conduit.Shared.AspNetCore.Controllers;

public abstract class SharedController : ControllerBase
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMediator _mediator;
    private ILogger? _logger;

    protected SharedController(
        IMediator mediator,
        ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _loggerFactory = loggerFactory;
    }

    private ILogger Logger =>
        _logger ??= _loggerFactory.CreateLogger(ControllerName);

    protected abstract string ControllerName { get; }

    protected async Task<IActionResult> Send<TResponse, TRequest, TResult>(
        TRequest request,
        Func<TResponse, IActionResult>? resultFactory = null,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse> where TResponse : Outcome<TResult>
    {
        Logger.LogInformation(EventIds.StartHandling,
            "Start handling request: {Request}", request);
        var response = await _mediator.Send(request, cancellationToken);
        LogResponse(request, response, response.Type);
        resultFactory ??= DefaultResultFactory<TResponse, TResult>;
        return resultFactory(response);
    }

    private IActionResult DefaultResultFactory<TResponse, TResult>(
        TResponse response) where TResponse : Outcome<TResult>
    {
        switch (response.Type)
        {
            case OutcomeType.Successful:
                if (response.Result is null)
                {
                    return NoContent();
                }

                return Ok(response.Result);
            case OutcomeType.Rejected:
                return
                    response is not FluentRejectedOutcome<TResult>
                        rejectedOutcome
                        ? new StatusCodeResult(422)
                        : new ObjectResult(new
                        {
                            errors = new ConduitCamelCaseSerializableError(
                                rejectedOutcome.ValidationResult
                                    .ToValidation()
                                    .ToModelStateDictionary())
                        }) { StatusCode = 422 };
            case OutcomeType.Failed:
                return StatusCode(StatusCodes.Status500InternalServerError);
            case OutcomeType.Banned:
                if (HttpContext.User.Identity?.IsAuthenticated ?? false)
                {
                    return Forbid();
                }

                return Unauthorized();
            default:
                throw new NotImplementedException(response.Type.ToString());
        }
    }

    private void LogResponse<TResponse, TRequest>(
        TRequest request,
        TResponse response,
        OutcomeType outcomeType)
    {
        switch (outcomeType)
        {
            case OutcomeType.Successful:
                Logger.LogInformation(EventIds.SuccessfulHandling,
                    "Successful handling request {Request} response {Response}",
                    request, response);
                break;
            case OutcomeType.Rejected:
                Logger.LogInformation(EventIds.RejectedHandling,
                    "Rejected request {Request} response {Response}", request,
                    response);
                break;
            case OutcomeType.Failed:
                Logger.LogError(EventIds.FailedHandling,
                    "Failed request {Request} response {Response}", request,
                    response);
                break;
            case OutcomeType.Banned:
                Logger.LogInformation(EventIds.BannedHandling,
                    "Banned request {Request} response {Response}", request,
                    response);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(outcomeType),
                    $"{nameof(outcomeType)} is invalid");
        }
    }

    private static class EventIds
    {
        public static EventId StartHandling => new(5221, "StartHandling");

        public static EventId SuccessfulHandling =>
            new(5211, "SuccessfulHandling");

        public static EventId RejectedHandling =>
            new(5212, "RejectedHandling");

        public static EventId FailedHandling => new(5213, "FailedHandling");

        public static EventId BannedHandling => new(5214, "BannedHandling");
    }
}
