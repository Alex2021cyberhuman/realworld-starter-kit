using Conduit.Images.DataAccess.Extensions;
using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Dapper;
using Npgsql;

namespace Conduit.Images.DataAccess.Articles;

public class ArticleConsumerRepository : IArticleConsumerRepository
{
    private readonly ConnectionProvider _connectionProvider;
    private readonly IImageStorage _storage;

    public ArticleConsumerRepository(
        ConnectionProvider connectionProvider,
        IImageStorage storage)
    {
        _connectionProvider = connectionProvider;
        _storage = storage;
    }

    public async Task CreateAsync(CreateArticleEventModel eventModel)
    {
        var connection = await _connectionProvider.ProvideAsync();
        const string createArticleQuery = @"INSERT INTO ""article"" (
            ""id"",
            ""slug"",
            ""author_id""
        ) VALUES (
            @Id,
            @Slug,
            @AuthorId
        );";
        await connection.ExecuteAsync(createArticleQuery, new
        {
            eventModel.Id,
            eventModel.Slug,
            eventModel.AuthorId
        });
    }

    public async Task RemoveAsync(DeleteArticleEventModel eventModel)
    {
        await RemoveByArticleImagesByIdArticleAsync(eventModel.Id);
        await RemoveArticleAsync(eventModel.Id);
    }

    private async Task RemoveArticleAsync(Guid id)
    {
        var connection = await _connectionProvider.ProvideAsync();
        const string removeArticleQuery = @"DELETE FROM ""article""
        WHERE ""id"" = @Id;";
        await connection.ExecuteAsync(removeArticleQuery, new
        {
            id
        }).SingleResult();
    }

    public async Task UpdateAsync(UpdateArticleEventModel eventModel)
    {
        var connection = await _connectionProvider.ProvideAsync();
        const string updateArticleQuery = @"UPDATE ""article""
        SET
            ""slug"" = @Slug,
            ""author_id"" = @AuthorId
        WHERE 
            ""id"" = @Id;";
        await connection.ExecuteAsync(updateArticleQuery, new
        {
            eventModel.Slug,
            eventModel.Id,
            eventModel.AuthorId
        }).SingleResult();
    }

    public async Task RemoveByArticleImagesByIdArticleAsync(Guid articleId)
    {
        var connection = await _connectionProvider.ProvideAsync();

        var articleImageStorageNames = await GetArticleImageStorageNamesAsync(articleId, connection);

        await _storage.MultipleRemoveAsync(articleImageStorageNames.ToArray());
        await DeleteArticleImagesAsync(articleId, connection);
    }

    private static async Task DeleteArticleImagesAsync(Guid articleId, NpgsqlConnection connection)
    {
        const string deleteAssignedArticleImagesCommand = @"DELETE ""article_image""
        WHERE ""article_id"" = @ArticleId";
        await connection.ExecuteAsync(deleteAssignedArticleImagesCommand, new { ArticleId = articleId });
    }

    private static async Task<IEnumerable<string>> GetArticleImageStorageNamesAsync(Guid articleId, NpgsqlConnection connection)
    {
        const string getArticleImageStorageNamesQuery = @"SELECT ""storage_name"" 
        FROM ""article_image"" WHERE ""article_id"" = @ArticleId";
        var articleImageStorageNames =
            await connection.QueryAsync<string>(getArticleImageStorageNamesQuery, new { ArticleId = articleId });
        return articleImageStorageNames;
    }
}
