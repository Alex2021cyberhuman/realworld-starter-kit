using Conduit.Likes.Domain.Articles;
using Conduit.Likes.Domain.Favorites;
using Conduit.Likes.Domain.Favorites.FavoriteArticle;
using Conduit.Likes.Domain.Shared;
using Conduit.Shared.Events.Models.Likes;
using Conduit.Shared.Events.Services;

namespace Conduit.Likes.BusinessLogic.Favorites.FavoriteArticle;

public class
    FavoriteArticleHandler : Domain.Favorites.FavoriteArticle.
        FavoriteArticleHandler
{
    private readonly IArticleRepository _articleRepository;

    private readonly IEventProducer<FavoriteArticleEventModel>
        _favoriteArticleEventProducer;

    private readonly IFavoritesRepository _favoritesRepository;

    public FavoriteArticleHandler(
        IFavoritesRepository favoritesRepository,
        IArticleRepository articleRepository,
        IEventProducer<FavoriteArticleEventModel> favoriteArticleEventProducer)
    {
        _favoritesRepository = favoritesRepository;
        _articleRepository = articleRepository;
        _favoriteArticleEventProducer = favoriteArticleEventProducer;
    }

    public override async Task<FavoriteArticleResponse> FavoriteAsync(
        FavoriteArticleRequest request)
    {
        var articleModel =
            await _articleRepository.FindArticleAsync(request.ArticleSlug);

        if (articleModel is null)
        {
            return new(Error.NotFound);
        }

        var favoriteAdditionResult =
            await _favoritesRepository.AddAsync(articleModel.Id,
                request.UserId);

        if (favoriteAdditionResult != Error.None)
        {
            return new(favoriteAdditionResult);
        }

        var favoriteArticleEventModel = new FavoriteArticleEventModel
        {
            ArticleId = articleModel.Id,
            UserId = request.UserId
        };

        await _favoriteArticleEventProducer.ProduceEventAsync(
            favoriteArticleEventModel);

        return new(Error.None);
    }
}
