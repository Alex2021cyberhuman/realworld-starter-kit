using Conduit.Shared.Events.Models.Users;

namespace Conduit.Person.BusinessLogic;

public interface IProfileRepository
{
    Task SaveAfterRegisterEventAsync(
        RegisterUserEventModel registerUserEventModel);

    Task SaveAfterUpdateEventAsync(
        UpdateUserEventModel updateUserEventModel);

    Task<(ProfileResponse?, Guid?)> FindAsync(
        FollowingInfo info);

    Task<(ProfileResponse?, Guid?)> AddFollowingAsync(
        FollowingInfo info);

    Task<(ProfileResponse?, Guid?)> RemoveFollowingAsync(
        FollowingInfo info);
}
