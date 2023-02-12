using StackExchange.Redis;

namespace Conduit.Likes.DataAccess.Favorites;

public static class FavoritesKeys
{
    public static RedisKey GetUserFavoritesKey(
        Guid userId)
    {
        return $"user:{userId:N}:favorites";
    }
}
