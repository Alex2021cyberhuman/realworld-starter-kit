using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Users;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class UpdateUserEventConsumer : IEventConsumer<UpdateUserEventModel>
{
    private readonly IAuthorConsumerRepository _authorConsumerRepository;

    public UpdateUserEventConsumer(
        IAuthorConsumerRepository authorConsumerRepository)
    {
        _authorConsumerRepository = authorConsumerRepository;
    }

    public async Task ConsumeAsync(
        UpdateUserEventModel message)
    {
        await _authorConsumerRepository.UpdateAsync(message);
    }
}
