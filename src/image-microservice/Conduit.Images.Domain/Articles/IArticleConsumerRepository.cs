using Conduit.Shared.Events.Models.Articles;

namespace Conduit.Images.Domain.Articles;

public interface IArticleConsumerRepository
{
    Task CreateAsync(
        CreateArticleEventModel eventModel);

    Task UpdateAsync(
        UpdateArticleEventModel eventModel);

    Task RemoveAsync(
        DeleteArticleEventModel eventModel);
}
