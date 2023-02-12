using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Likes;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class
    FavoriteArticleEventConsumer : IEventConsumer<FavoriteArticleEventModel>
{
    private readonly IFavoritesConsumerRepository _favoritesConsumerRepository;

    public FavoriteArticleEventConsumer(
        IFavoritesConsumerRepository favoritesConsumerRepository)
    {
        _favoritesConsumerRepository = favoritesConsumerRepository;
    }

    public async Task ConsumeAsync(
        FavoriteArticleEventModel message)
    {
        await _favoritesConsumerRepository.FavoriteAsync(message);
    }
}
