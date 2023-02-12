using Conduit.Articles.DomainLayer.Models;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IArticleReadRepository
{
    Task<SingleArticle> FindAsync(
        FindArticle.Request request,
        CancellationToken cancellationToken = default);

    Task<MultipleArticles> SearchAsync(
        SearchArticles.Request request,
        CancellationToken cancellationToken = default);

    Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default);
}
