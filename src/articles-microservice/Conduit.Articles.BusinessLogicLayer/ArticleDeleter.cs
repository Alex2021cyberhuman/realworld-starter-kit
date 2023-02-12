using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Articles;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleDeleter : IArticleDeleter
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IEventProducer<DeleteArticleEventModel> _eventProducer;

    public ArticleDeleter(
        IArticleWriteRepository articleWriteRepository,
        IEventProducer<DeleteArticleEventModel> eventProducer)
    {
        _articleWriteRepository = articleWriteRepository;
        _eventProducer = eventProducer;
    }

    public async Task DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken)
    {
        var internalArticleModel =
            await _articleWriteRepository.DeleteAsync(article,
                cancellationToken);
        await ProduceEventAsync(internalArticleModel);
    }

    private async Task ProduceEventAsync(
        InternalArticleModel article)
    {
        await _eventProducer.ProduceEventAsync(new()
        {
            Id = article.Id,
            UserId = article.Author.Id
        });
    }
}
