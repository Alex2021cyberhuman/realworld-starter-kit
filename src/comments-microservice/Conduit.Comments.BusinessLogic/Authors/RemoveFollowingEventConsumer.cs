using Conduit.Comments.Domain.Authors;
using Conduit.Shared.Events.Models.Profiles.RemoveFollowing;
using Conduit.Shared.Events.Services;
using Microsoft.Extensions.Logging;

namespace Conduit.Comments.BusinessLogic.Authors;

public class
    RemoveFollowingEventConsumer : IEventConsumer<RemoveFollowingEventModel>
{
    private readonly ILogger<RemoveFollowingEventConsumer> _logger;
    private readonly IAuthorConsumerRepository _repository;

    public RemoveFollowingEventConsumer(
        IAuthorConsumerRepository repository,
        ILogger<RemoveFollowingEventConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ConsumeAsync(
        RemoveFollowingEventModel message)
    {
        _logger.LogTrace("Received remove following event {Message}", message);
        await _repository.RemoveFollowingAsync(message);
        _logger.LogTrace("Following removed {FollowerId} {FollowedId}",
            message.FollowerId, message.FollowedId);
    }
}
