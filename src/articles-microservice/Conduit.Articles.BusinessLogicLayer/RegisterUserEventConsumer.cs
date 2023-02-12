using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Users;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class RegisterUserEventConsumer : IEventConsumer<RegisterUserEventModel>
{
    private readonly IAuthorConsumerRepository _authorConsumerRepository;

    public RegisterUserEventConsumer(
        IAuthorConsumerRepository authorConsumerRepository)
    {
        _authorConsumerRepository = authorConsumerRepository;
    }

    public async Task ConsumeAsync(
        RegisterUserEventModel message)
    {
        await _authorConsumerRepository.RegisterAsync(message);
    }
}
