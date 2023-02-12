using Conduit.Likes.Domain.Favorites;
using Conduit.Likes.Domain.Shared;

namespace Conduit.Likes.DataAccess.Favorites;

public class FavoritesRepository : IFavoritesRepository
{
    private readonly ConnectionProvider _connectionProvider;

    public FavoritesRepository(
        ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Error> AddAsync(
        Guid articleId,
        Guid userId)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var userFavoritesKey = FavoritesKeys.GetUserFavoritesKey(userId);
        var newUserFavoritesValue = articleId.ToString("N");
        var added = await database.SetAddAsync(
            userFavoritesKey, newUserFavoritesValue);
        return added ? Error.None : Error.BadRequest;
    }

    public async Task<Error> RemoveAsync(
        Guid articleId,
        Guid userId)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var removed = await database.SetRemoveAsync(
            FavoritesKeys.GetUserFavoritesKey(userId), articleId.ToString("N"));
        return removed ? Error.None : Error.BadRequest;
    }
}
