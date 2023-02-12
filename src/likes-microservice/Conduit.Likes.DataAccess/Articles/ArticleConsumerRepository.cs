using Conduit.Likes.Domain.Articles;
using Conduit.Shared.Events.Models.Articles;
using StackExchange.Redis;

namespace Conduit.Likes.DataAccess.Articles;

public class ArticleConsumerRepository : IArticleConsumerRepository
{
    private readonly ConnectionProvider _connectionProvider;

    public ArticleConsumerRepository(
        ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task CreateAsync(
        CreateArticleEventModel model)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var transaction = database.CreateTransaction();
        var articleSlugKey = ArticlesKeys.GetArticleSlugKey(model.Slug);
        var articleSlugReferenceKey = ArticlesKeys.GetArticleIdKey(model.Id);
        var setSlugReferenceTask =
            transaction.StringSetAsync(articleSlugReferenceKey,
                articleSlugKey.ToString());
        var hashEntries = GetHashEntries(model.Id, model.Slug);
        var hashSetArticleInformationTask =
            transaction.HashSetAsync(articleSlugKey, hashEntries);
        var transactionTask = transaction.ExecuteAsync();
        await Task.WhenAll(setSlugReferenceTask, hashSetArticleInformationTask,
            transactionTask);
    }

    public async Task UpdateAsync(
        UpdateArticleEventModel model)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var transaction = database.CreateTransaction();
        var articleSlugReferenceKey = ArticlesKeys.GetArticleIdKey(model.Id);
        var oldArticleSlugKey =
            new RedisKey(
                await transaction.StringGetAsync(articleSlugReferenceKey));
        var hashFields = ArticlesKeys.GetArticleFields();
        var removeOldArticleInformationTask =
            transaction.HashDeleteAsync(oldArticleSlugKey, hashFields);
        var articleSlugKey = ArticlesKeys.GetArticleSlugKey(model.Slug);
        var setSlugReferenceTask =
            transaction.StringSetAsync(articleSlugReferenceKey,
                articleSlugKey.ToString());
        var hashEntries = GetHashEntries(model.Id, model.Slug);
        var hashSetArticleInformationTask =
            transaction.HashSetAsync(articleSlugKey, hashEntries);
        var transactionTask = transaction.ExecuteAsync();
        await Task.WhenAll(setSlugReferenceTask, hashSetArticleInformationTask,
            removeOldArticleInformationTask, transactionTask);
    }

    public async Task RemoveAsync(
        DeleteArticleEventModel model)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var transaction = database.CreateTransaction();
        var articleSlugReferenceKey = ArticlesKeys.GetArticleIdKey(model.Id);
        var oldArticleSlugKey =
            new RedisKey(
                await transaction.StringGetAsync(articleSlugReferenceKey));
        var hashFields = ArticlesKeys.GetArticleFields();
        var removeOldArticleInformationTask =
            transaction.HashDeleteAsync(oldArticleSlugKey, hashFields);
        var transactionTask = transaction.ExecuteAsync();
        await Task.WhenAll(removeOldArticleInformationTask, transactionTask);
    }

    public static HashEntry[] GetHashEntries(
        Guid id,
        string slug)
    {
        var hashEntries = new HashEntry[]
        {
            new(ArticlesKeys.ArticleId, id.ToString("N")),
            new(ArticlesKeys.ArticleSlug, slug)
        };
        return hashEntries;
    }
}
