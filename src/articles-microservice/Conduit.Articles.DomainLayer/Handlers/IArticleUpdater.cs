using Conduit.Articles.DomainLayer.Models;

namespace Conduit.Articles.DomainLayer.Handlers;

public interface IArticleUpdater
{
    Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken);
}
