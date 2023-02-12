namespace Conduit.Likes.Domain.Articles;

public interface IArticleRepository
{
    Task<ArticleModel?> FindArticleAsync(
        string articleSlug);
}
