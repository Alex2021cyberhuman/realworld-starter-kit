namespace Conduit.Likes.Domain.Favorites.FavoriteArticle;

public abstract class FavoriteArticleHandler
{
    public abstract Task<FavoriteArticleResponse> FavoriteAsync(
        FavoriteArticleRequest request);
}
