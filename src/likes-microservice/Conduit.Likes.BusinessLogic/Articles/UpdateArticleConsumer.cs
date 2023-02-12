using Conduit.Likes.Domain.Articles;
using Conduit.Shared.Events.Services;

namespace Conduit.Likes.BusinessLogic.Articles;

public class UpdateArticleConsumer : IEventConsumer<UpdateArticleEventModel>
{
    private readonly IArticleConsumerRepository _articleConsumeRepository;

    public UpdateArticleConsumer(
        IArticleConsumerRepository articleConsumeRepository)
    {
        _articleConsumeRepository = articleConsumeRepository;
    }

    public async Task ConsumeAsync(
        UpdateArticleEventModel message)
    {
        await _articleConsumeRepository.UpdateAsync(message);
    }
}
