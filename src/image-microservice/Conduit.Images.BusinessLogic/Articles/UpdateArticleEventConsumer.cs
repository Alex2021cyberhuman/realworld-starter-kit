using Conduit.Images.Domain.Articles;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Conduit.Shared.Events.Services;
using Microsoft.Extensions.Logging;

namespace Conduit.Images.BusinessLogic.Articles;

public class
    UpdateArticleEventConsumer : IEventConsumer<UpdateArticleEventModel>
{
    private readonly IArticleConsumerRepository _articleConsumeRepository;
    private readonly ILogger<UpdateArticleEventConsumer> _logger;

    public UpdateArticleEventConsumer(
        IArticleConsumerRepository articleConsumeRepository,
        ILogger<UpdateArticleEventConsumer> logger)
    {
        _articleConsumeRepository = articleConsumeRepository;
        _logger = logger;
    }

    public async Task ConsumeAsync(
        UpdateArticleEventModel message)
    {
        _logger.LogTrace("Received update article model {Message}", message);
        await _articleConsumeRepository.UpdateAsync(message);
        _logger.LogTrace("Article updated {Id}", message.Id);
    }
}
