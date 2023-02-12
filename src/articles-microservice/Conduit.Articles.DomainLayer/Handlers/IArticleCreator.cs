using Conduit.Articles.DomainLayer.Models;

namespace Conduit.Articles.DomainLayer.Handlers;

public interface IArticleCreator
{
    Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken);
}
