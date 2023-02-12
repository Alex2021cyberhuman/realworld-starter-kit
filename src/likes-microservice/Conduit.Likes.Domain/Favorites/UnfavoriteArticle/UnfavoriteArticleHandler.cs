namespace Conduit.Likes.Domain.Favorites.UnfavoriteArticle;

public abstract class UnfavoriteArticleHandler
{
    public abstract Task<UnfavoriteArticleResponse> UnfavoriteArticleAsync(
        UnfavoriteArticleRequest request);
}
