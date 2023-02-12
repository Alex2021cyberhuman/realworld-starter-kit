using Conduit.Articles.DomainLayer.Models;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IArticleWriteRepository
{
    Task<InternalArticleModel> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken = default);

    Task<InternalArticleModel> UpdateAsync(
        UpdateArticle.Request request,
        CancellationToken cancellationToken = default);

    Task<InternalArticleModel> DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken = default);
}
