using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Articles.DomainLayer.Utilities;
using Conduit.Shared.Events.Models.Articles;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleUpdater : IArticleUpdater
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IEventProducer<UpdateArticleEventModel> _eventProducer;
    private readonly IValidator<UpdateArticle.Request> _validator;

    public ArticleUpdater(
        IArticleWriteRepository articleWriteRepository,
        IValidator<UpdateArticle.Request> validator,
        IEventProducer<UpdateArticleEventModel> eventProducer)
    {
        _articleWriteRepository = articleWriteRepository;
        _validator = validator;
        _eventProducer = eventProducer;
    }

    public async Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(article, cancellationToken);
        var internalArticleModel =
            await _articleWriteRepository.UpdateAsync(article,
                cancellationToken);
        await ProduceEvent(internalArticleModel);
        return new(internalArticleModel);
    }

    private async Task ProduceEvent(
        InternalArticleModel article)
    {
        await _eventProducer.ProduceEventAsync(new()
        {
            Id = article.Id,
            Slug = article.Slug,
            Title = article.Title,
            Description = article.Description,
            Body = article.Body,
            TagList = article.TagList,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            AuthorId = article.Author.Id
        });
    }
}
