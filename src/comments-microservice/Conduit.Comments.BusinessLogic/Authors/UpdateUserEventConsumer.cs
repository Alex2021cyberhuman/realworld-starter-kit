using Conduit.Comments.Domain.Authors;
using Conduit.Shared.Events.Models.Users.Update;
using Conduit.Shared.Events.Services;
using Microsoft.Extensions.Logging;

namespace Conduit.Comments.BusinessLogic.Authors;

public class UpdateUserEventConsumer : IEventConsumer<UpdateUserEventModel>
{
    private readonly ILogger<UpdateUserEventConsumer> _logger;
    private readonly IAuthorConsumerRepository _repository;

    public UpdateUserEventConsumer(
        IAuthorConsumerRepository repository,
        ILogger<UpdateUserEventConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ConsumeAsync(
        UpdateUserEventModel message)
    {
        _logger.LogTrace("Received user update event {Message}", message);
        await _repository.UpdateAsync(message);
        _logger.LogTrace("User updated with id {Id}", message.Id);
    }
}
