using Conduit.Comments.Domain.Authors;
using Conduit.Shared.Events.Models.Profiles.CreateFollowing;
using Conduit.Shared.Events.Services;
using Microsoft.Extensions.Logging;

namespace Conduit.Comments.BusinessLogic.Authors;

public class
    CreateFollowingEventConsumer : IEventConsumer<CreateFollowingEventModel>
{
    private readonly ILogger<CreateFollowingEventConsumer> _logger;
    private readonly IAuthorConsumerRepository _repository;

    public CreateFollowingEventConsumer(
        IAuthorConsumerRepository repository,
        ILogger<CreateFollowingEventConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ConsumeAsync(
        CreateFollowingEventModel message)
    {
        _logger.LogTrace("Received create following event {Message}", message);
        await _repository.CreateFollowingAsync(message);
        _logger.LogTrace("Following created {FollowerId} {FollowedId}",
            message.FollowerId, message.FollowedId);
    }
}
