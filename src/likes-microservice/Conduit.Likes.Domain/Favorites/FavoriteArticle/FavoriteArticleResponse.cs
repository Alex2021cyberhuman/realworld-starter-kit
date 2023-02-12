using Conduit.Likes.Domain.Shared;

namespace Conduit.Likes.Domain.Favorites.FavoriteArticle;

public class FavoriteArticleResponse : BaseResponse
{
    public FavoriteArticleResponse(
        Error error) : base(error)
    {
    }
}
