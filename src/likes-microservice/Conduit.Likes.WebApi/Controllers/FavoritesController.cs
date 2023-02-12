using System.ComponentModel.DataAnnotations;
using Conduit.Likes.Domain.Favorites.FavoriteArticle;
using Conduit.Likes.Domain.Favorites.UnfavoriteArticle;
using Conduit.Likes.Domain.Shared;
using Conduit.Shared.AuthorizationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Likes.WebApi.Controllers;

[Route("articles/{slug}/favorite")]
public class FavoritesController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> FavoriteArticle(
        [FromServices] FavoriteArticleHandler favoriteArticleHandler,
        [FromRoute][Required] string slug)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new FavoriteArticleRequest
        {
            ArticleSlug = slug,
            UserId = userId
        };
        var response = await favoriteArticleHandler.FavoriteAsync(request);
        var error = response.Error;
        var actionResult = SwitchActionResult(error);
        return actionResult;
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> FavoriteArticle(
        [FromServices] UnfavoriteArticleHandler unfavoriteArticleHandler,
        [FromRoute][Required] string slug)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new UnfavoriteArticleRequest
        {
            ArticleSlug = slug,
            UserId = userId
        };
        var response =
            await unfavoriteArticleHandler.UnfavoriteArticleAsync(request);
        var error = response.Error;
        var actionResult = SwitchActionResult(error);
        return actionResult;
    }

    private static IActionResult SwitchActionResult(
        Error error)
    {
        return error switch
        {
            Error.None => new OkResult(),
            Error.NotFound => new NotFoundResult(),
            Error.BadRequest => new BadRequestResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}
