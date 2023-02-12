namespace Conduit.Likes.Domain.Favorites.UnfavoriteArticle;

public class UnfavoriteArticleRequest
{
    public string ArticleSlug { get; set; } = string.Empty;

    public Guid UserId { get; set; }
}
