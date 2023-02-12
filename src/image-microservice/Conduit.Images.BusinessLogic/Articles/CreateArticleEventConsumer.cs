using Conduit.Images.Domain.Articles;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Services;
using Microsoft.Extensions.Logging;
namespace Conduit.Images.BusinessLogic.Articles;

public class CreateArticleEventConsumer : IEventConsumer<CreateArticleEventModel>
{
    private readonly IArticleConsumerRepository _articleConsumeRepository;
    private readonly ILogger<CreateArticleEventConsumer> _logger;

    public CreateArticleEventConsumer(
        IArticleConsumerRepository articleConsumeRepository,
        ILogger<CreateArticleEventConsumer> logger)
    {
        _articleConsumeRepository = articleConsumeRepository;
        _logger = logger;
    }

    public async Task ConsumeAsync(
        CreateArticleEventModel message)
    {
        _logger.LogTrace("Received create article model {Message}", message);
        await _articleConsumeRepository.CreateAsync(message);
        _logger.LogTrace("Article added with id {Id}", message.Id);
    }
}
