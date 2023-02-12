using Conduit.Likes.Domain.Articles;

namespace Conduit.Likes.DataAccess.Articles;

public class ArticlesRepository : IArticleRepository
{
    private readonly ConnectionProvider _connectionProvider;

    public ArticlesRepository(
        ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<ArticleModel?> FindArticleAsync(
        string articleSlug)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var articleModel = await database.FindArticleAsync(articleSlug);
        return articleModel;
    }
}
