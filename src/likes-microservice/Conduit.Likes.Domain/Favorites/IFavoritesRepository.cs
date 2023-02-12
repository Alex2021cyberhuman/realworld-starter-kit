using Conduit.Likes.Domain.Shared;

namespace Conduit.Likes.Domain.Favorites;

public interface IFavoritesRepository
{
    Task<Error> AddAsync(
        Guid articleId,
        Guid userId);

    Task<Error> RemoveAsync(
        Guid articleId,
        Guid userId);
}
