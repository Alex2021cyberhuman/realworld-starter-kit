using Conduit.Articles.DomainLayer.Models;

namespace Conduit.Articles.DomainLayer.Handlers;

public interface IArticleDeleter
{
    Task DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken);
}
