using Conduit.Shared.Events.Models.Users;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IAuthorConsumerRepository
{
    Task RegisterAsync(
        RegisterUserEventModel model);

    Task UpdateAsync(
        UpdateUserEventModel model);
}
