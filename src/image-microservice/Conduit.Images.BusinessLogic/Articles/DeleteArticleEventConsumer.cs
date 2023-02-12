using Conduit.Images.Domain.Articles;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Services;

using Microsoft.Extensions.Logging;

namespace Conduit.Images.BusinessLogic.Articles;

public class
    DeleteArticleEventConsumer : IEventConsumer<DeleteArticleEventModel>
{
    private readonly IArticleConsumerRepository _articleConsumeRepository;
    private readonly ILogger<DeleteArticleEventConsumer> _logger;

    public DeleteArticleEventConsumer(
        IArticleConsumerRepository articleConsumeRepository,
        ILogger<DeleteArticleEventConsumer> logger)
    {
        _articleConsumeRepository = articleConsumeRepository;
        _logger = logger;
    }

    public async Task ConsumeAsync(
        DeleteArticleEventModel message)
    {
        _logger.LogTrace("Received delete article model {Message}", message);
        await _articleConsumeRepository.RemoveAsync(message);
        _logger.LogTrace("Article removed with id {Id}", message.Id);
    }
}
