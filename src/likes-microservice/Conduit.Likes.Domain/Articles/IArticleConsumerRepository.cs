using Conduit.Shared.Events.Models.Articles;

namespace Conduit.Likes.Domain.Articles;

public interface IArticleConsumerRepository
{
    Task CreateAsync(
        CreateArticleEventModel model);

    Task RemoveAsync(
        DeleteArticleEventModel model);

    Task UpdateAsync(
        UpdateArticleEventModel model);
}
