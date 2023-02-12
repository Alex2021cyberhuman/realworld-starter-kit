namespace Conduit.Likes.Domain.Favorites.FavoriteArticle;

public class FavoriteArticleRequest
{
    public string ArticleSlug { get; set; } = string.Empty;

    public Guid UserId { get; set; }
}
