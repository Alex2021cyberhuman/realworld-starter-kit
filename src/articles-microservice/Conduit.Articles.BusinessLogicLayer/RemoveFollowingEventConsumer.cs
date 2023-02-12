using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Profiles;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class
    RemoveFollowingEventConsumer : IEventConsumer<RemoveFollowingEventModel>
{
    private readonly IFollowingsConsumerRepository
        _followingsConsumerRepository;

    public RemoveFollowingEventConsumer(
        IFollowingsConsumerRepository followingsConsumerRepository)
    {
        _followingsConsumerRepository = followingsConsumerRepository;
    }

    public async Task ConsumeAsync(
        RemoveFollowingEventModel message)
    {
        await _followingsConsumerRepository.RemoveAsync(message);
    }
}
