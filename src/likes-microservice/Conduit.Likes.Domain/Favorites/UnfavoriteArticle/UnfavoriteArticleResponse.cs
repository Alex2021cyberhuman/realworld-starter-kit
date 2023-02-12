using Conduit.Likes.Domain.Shared;

namespace Conduit.Likes.Domain.Favorites.UnfavoriteArticle;

public class UnfavoriteArticleResponse : BaseResponse
{
    public UnfavoriteArticleResponse(
        Error error) : base(error)
    {
    }
}
