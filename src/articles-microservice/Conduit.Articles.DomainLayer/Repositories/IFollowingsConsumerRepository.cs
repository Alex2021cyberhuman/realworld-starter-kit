using Conduit.Shared.Events.Models.Profiles;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IFollowingsConsumerRepository
{
    Task CreateAsync(
        CreateFollowingEventModel model);

    Task RemoveAsync(
        RemoveFollowingEventModel model);
}
