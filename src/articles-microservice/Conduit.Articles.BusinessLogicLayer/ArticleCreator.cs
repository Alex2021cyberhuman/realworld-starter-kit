using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Articles.DomainLayer.Utilities;
using Conduit.Shared.Events.Models.Articles;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleCreator : IArticleCreator
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IEventProducer<CreateArticleEventModel> _eventProducer;
    private readonly IValidator<CreateArticle.Request> _validator;

    public ArticleCreator(
        IArticleWriteRepository articleWriteRepository,
        IValidator<CreateArticle.Request> validator,
        IEventProducer<CreateArticleEventModel> eventProducer)
    {
        _articleWriteRepository = articleWriteRepository;
        _validator = validator;
        _eventProducer = eventProducer;
    }

    public async Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(article, cancellationToken);
        var internalArticleModel =
            await _articleWriteRepository.CreateAsync(article,
                cancellationToken);
        await ProduceEvent(internalArticleModel);
        return new(internalArticleModel);
    }

    private async Task ProduceEvent(
        InternalArticleModel internalArticleModel)
    {
        await _eventProducer.ProduceEventAsync(new()
        {
            Id = internalArticleModel.Id,
            Slug = internalArticleModel.Slug,
            Title = internalArticleModel.Title,
            Description = internalArticleModel.Description,
            Body = internalArticleModel.Body,
            TagList = internalArticleModel.TagList,
            CreatedAt = internalArticleModel.CreatedAt,
            UpdatedAt = internalArticleModel.UpdatedAt,
            AuthorId = internalArticleModel.Author.Id
        });
    }
}
