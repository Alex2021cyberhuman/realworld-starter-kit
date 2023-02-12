using Conduit.Shared.Events.Models.Profiles;
using Conduit.Shared.Events.Models.Users;

namespace Conduit.Comments.Domain.Authors;

public interface IAuthorConsumerRepository
{
    Task RegisterAsync(
        RegisterUserEventModel eventModel);

    Task UpdateAsync(
        UpdateUserEventModel eventModel);

    Task CreateFollowingAsync(
        CreateFollowingEventModel eventModel);

    Task RemoveFollowingAsync(
        RemoveFollowingEventModel eventModel);
}
