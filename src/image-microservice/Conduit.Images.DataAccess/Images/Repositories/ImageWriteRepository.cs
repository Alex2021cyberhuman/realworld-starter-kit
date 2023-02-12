using Conduit.Images.DataAccess.Extensions;
using Conduit.Images.DataAccess.Images.Models;
using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.Common;
using Conduit.Images.Domain.Images.Services.Repositories;
using Dapper;

namespace Conduit.Images.DataAccess.Images.Repositories;

public class ImageWriteRepository : IImageWriteRepository
{
    private readonly IImageStorage _imageStorage;
    private readonly ConnectionProvider _connectionProvider;

    public ImageWriteRepository(
        IImageStorage imageStorage,
        ConnectionProvider connectionProvider)
    {
        _imageStorage = imageStorage;
        _connectionProvider = connectionProvider;
    }

    public async Task AssignAsync(ArticleDomainModel article, ISet<Guid> imageIds, CancellationToken cancellationToken = default)
    {
        const string assignArticleImageCommand = @"UPDATE ""article_image""
        SET ""article_id"" = @ArticleId
        WHERE ""id"" IN @ImageIds";
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        await connection.ExecuteAsync(assignArticleImageCommand, new { ImageIds = imageIds, ArticleId = article.Id });
    }

    public async Task RemoveAsync(ArticleImageDomainModel articleImageDomainModel, CancellationToken cancellationToken = default)
    {
        const string removeArticleImageCommand = @"DELETE FROM ""article_image""
        WHERE ""id"" = @Id";
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        await connection.ExecuteAsync(removeArticleImageCommand, new { articleImageDomainModel.Id }).SingleResult();
        await _imageStorage.RemoveAsync(articleImageDomainModel.StorageName, cancellationToken);
    }

    public async Task RemoveUnassignedOlderThanAsync(DateTime dateTime, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        const string getArticleImageStorageNamesOlderThenDateTimeQuery = @"SELECT ""storage_name"" FROM ""article_image"" 
        WHERE  ""uploaded"" < @DateTime AND ""article_id"" IS NULL";
        var names = await connection.QueryAsync<string>(getArticleImageStorageNamesOlderThenDateTimeQuery);
        await _imageStorage.MultipleRemoveAsync(names.ToList(), cancellationToken);
        const string removeArticleImageOlderThanDateTimeCommand = @"DELETE FROM ""article_image""
        WHERE ""uploaded"" < @DateTime AND ""article_id"" IS NULL";
        await connection.ExecuteAsync(removeArticleImageOlderThanDateTimeCommand, new { DateTime = dateTime });
    }

    public async Task<ArticleImageDomainModel> SaveAsync(
        Guid userId,
        Stream stream,
        string mediaType,
        Guid imageId,
        string storageName,
        string imageUrl,
        DateTime uploaded,
        CancellationToken cancellationToken = default)
    {
        await _imageStorage.StoreAsync(storageName, stream, cancellationToken);
        var dbModel = await InsertAsync(imageId, userId, storageName, mediaType, uploaded, cancellationToken);
        return dbModel.ToDomainModel(imageUrl);
    }

    private async Task<ArticleImageDbModel> InsertAsync(
        Guid imageId,
        Guid userId,
        string storageName,
        string mediaType,
        DateTime uploaded,
        CancellationToken cancellationToken)
    {
        var dbModel = new ArticleImageDbModel(imageId, userId, storageName, mediaType, uploaded);
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        const string insertArticleImageCommand = @"INSERT INTO ""article_image"" (
            ""id"",
            ""user_id"",
            ""storage_name"",
            ""media_type"",
            ""uploaded""
        ) VALUES (
            @Id,
            @UserId,
            @StorageName,
            @MediaType,
            @Uploaded
        );";
        await connection.ExecuteAsync(insertArticleImageCommand, dbModel).SingleResult();
        return dbModel;
    }
}
