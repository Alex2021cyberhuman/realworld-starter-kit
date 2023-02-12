using Conduit.Comments.Domain.Authors;
using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Services;
using Microsoft.Extensions.Logging;

namespace Conduit.Comments.BusinessLogic.Authors;

public class RegisterUserEventConsumer : IEventConsumer<RegisterUserEventModel>
{
    private readonly ILogger<RegisterUserEventConsumer> _logger;
    private readonly IAuthorConsumerRepository _repository;

    public RegisterUserEventConsumer(
        IAuthorConsumerRepository repository,
        ILogger<RegisterUserEventConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ConsumeAsync(
        RegisterUserEventModel message)
    {
        _logger.LogTrace("Received register user event {Message}", message);
        await _repository.RegisterAsync(message);
        _logger.LogTrace("User registered {Id}", message.Id);
    }
}
