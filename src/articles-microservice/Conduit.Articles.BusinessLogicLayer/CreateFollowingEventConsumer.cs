using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Profiles;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class
    CreateFollowingEventConsumer : IEventConsumer<CreateFollowingEventModel>
{
    private readonly IFollowingsConsumerRepository
        _followingsConsumerRepository;

    public CreateFollowingEventConsumer(
        IFollowingsConsumerRepository followingsConsumerRepository)
    {
        _followingsConsumerRepository = followingsConsumerRepository;
    }

    public async Task ConsumeAsync(
        CreateFollowingEventModel message)
    {
        await _followingsConsumerRepository.CreateAsync(message);
    }
}
