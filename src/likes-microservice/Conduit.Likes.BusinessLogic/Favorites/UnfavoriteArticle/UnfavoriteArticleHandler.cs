using Conduit.Likes.Domain.Articles;
using Conduit.Likes.Domain.Favorites;
using Conduit.Likes.Domain.Favorites.UnfavoriteArticle;
using Conduit.Likes.Domain.Shared;
using Conduit.Shared.Events.Models.Likes;
using Conduit.Shared.Events.Services;

namespace Conduit.Likes.BusinessLogic.Favorites.UnfavoriteArticle;

public class
    UnfavoriteArticleHandler : Domain.Favorites.UnfavoriteArticle.
        UnfavoriteArticleHandler
{
    private readonly IArticleRepository _articleRepository;
    private readonly IFavoritesRepository _favoritesRepository;

    private readonly IEventProducer<UnfavoriteArticleEventModel>
        _unfavoriteArticleEventProducer;

    public UnfavoriteArticleHandler(
        IFavoritesRepository favoritesRepository,
        IArticleRepository articleRepository,
        IEventProducer<UnfavoriteArticleEventModel>
            unfavoriteArticleEventProducer)
    {
        _favoritesRepository = favoritesRepository;
        _articleRepository = articleRepository;
        _unfavoriteArticleEventProducer = unfavoriteArticleEventProducer;
    }

    public override async Task<UnfavoriteArticleResponse>
        UnfavoriteArticleAsync(
            UnfavoriteArticleRequest request)
    {
        var articleModel =
            await _articleRepository.FindArticleAsync(request.ArticleSlug);

        if (articleModel is null)
        {
            return new(Error.NotFound);
        }

        var favoriteRemovingResult =
            await _favoritesRepository.RemoveAsync(articleModel.Id,
                request.UserId);

        if (favoriteRemovingResult != Error.None)
        {
            return new(favoriteRemovingResult);
        }

        var unfavoriteArticleEventModel = new UnfavoriteArticleEventModel
        {
            ArticleId = articleModel.Id,
            UserId = request.UserId
        };

        await _unfavoriteArticleEventProducer.ProduceEventAsync(
            unfavoriteArticleEventModel);

        return new(Error.None);
    }
}
