using Conduit.Images.Domain.Articles;
using Dapper;

namespace Conduit.Images.DataAccess.Articles;

public class ArticleReadRepository : IArticleReadRepository
{
    private readonly ConnectionProvider _connectionProvider;

    public ArticleReadRepository(ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<ArticleDomainModel?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        const string findArticleQuery = @"SELECT 
            ""id"",
            ""slug"",
            ""author_id""
        FROM ""article""
        WHERE ""id"" = @Id";
        var result = await connection.QueryFirstOrDefaultAsync<ArticleDbModel>(findArticleQuery, new { Id = id });
        return result?.ToArticleDomainModel();
    }
}
