using Conduit.Likes.Domain.Articles;
using Conduit.Shared.Events.Models.Articles;
using Conduit.Shared.Events.Services;

namespace Conduit.Likes.BusinessLogic.Articles;

public class CreateArticleConsumer : IEventConsumer<CreateArticleEventModel>
{
    private readonly IArticleConsumerRepository _articleConsumeRepository;

    public CreateArticleConsumer(
        IArticleConsumerRepository articleConsumeRepository)
    {
        _articleConsumeRepository = articleConsumeRepository;
    }

    public async Task ConsumeAsync(
        CreateArticleEventModel message)
    {
        await _articleConsumeRepository.CreateAsync(message);
    }
}
