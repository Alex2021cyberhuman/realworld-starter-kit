using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Conduit.Auth.ApplicationLayer
{
    public class PipelineLogger<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull where TResponse : ITypedOutcome
    {
        private readonly ILogger<PipelineLogger<TRequest, TResponse>> _logger;

        public PipelineLogger(ILogger<PipelineLogger<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation(EventIds.StartHandling, "Start handling request: {Request}", request);

            var result = await next();

            LogResponse(request, result.Type);

            return result;
        }

        private void LogResponse(TRequest request, OutcomeType outcomeType)
        {
            switch (outcomeType)
            {
                case OutcomeType.Successful:
                    _logger.LogInformation(
                        EventIds.SuccessfulHandling,
                        "Successful handling request: {Request}",
                        request);
                    break;
                case OutcomeType.Rejected:
                    _logger.LogInformation(
                        EventIds.RejectedHandling,
                        "Rejected request: {Request}",
                        request);
                    break;
                case OutcomeType.Failed:
                    _logger.LogError(
                        EventIds.FailedHandling,
                        "Failed request: {Request}",
                        request);
                    break;
                case OutcomeType.Banned:
                    _logger.LogInformation(
                        EventIds.BannedHandling,
                        "Banned request: {Request}",
                        request);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(outcomeType), "outcomeType is invalid");
            }
        }

        public static class EventIds
        {
            public static EventId StartHandling =>
                new EventId(5221, "StartHandling");
            
            public static EventId SuccessfulHandling =>
                new EventId(5211, "SuccessfulHandling");
            
            public static EventId RejectedHandling =>
                new EventId(5212, "RejectedHandling");
            
            public static EventId FailedHandling =>
                new EventId(5213, "FailedHandling");
            
            public static EventId BannedHandling =>
                new EventId(5214, "BannedHandling");
        }
    }
}
