using System.Net;
using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.AuthorizationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Articles.PresentationLayer.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    [Authorize]
    [HttpPost]
    [Produces(typeof(SingleArticle.ResponseBody))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateArticle(
        [FromServices] IArticleCreator creator,
        [FromBody] CreateArticle.RequestBody requestBody,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new CreateArticle.Request(requestBody, userId);
        var result = await creator.CreateAsync(request, cancellationToken);
        return Ok(result.Response);
    }

    [Authorize]
    [HttpPut("{slug}")]
    [Produces(typeof(SingleArticle.ResponseBody))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdateArticle(
        [FromServices] IArticleUpdater updater,
        [FromRoute] string slug,
        [FromBody] UpdateArticle.RequestBody requestBody,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new UpdateArticle.Request(requestBody, slug, userId);
        var result = await updater.UpdateAsync(request, cancellationToken);
        return Ok(result.Response);
    }

    [HttpGet("{slug}")]
    [Produces(typeof(SingleArticle.ResponseBody))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> FindArticle(
        [FromServices] IArticleReadRepository repository,
        [FromRoute] string slug,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserIdOptional();
        var request = new FindArticle.Request(userId, slug);
        var result = await repository.FindAsync(request, cancellationToken);
        return Ok(result.Response);
    }

    [HttpDelete("{slug}")]
    [Produces(typeof(SingleArticle.ResponseBody))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteArticle(
        [FromServices] IArticleDeleter deleter,
        [FromRoute] string slug,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new DeleteArticle.Request(userId, slug);
        await deleter.DeleteAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [Produces(typeof(MultipleArticles.ResponseBody))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> SearchArticles(
        [FromServices] IArticleReadRepository repository,
        [FromQuery] SearchArticles.QueryParams query,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserIdOptional();
        var request = new SearchArticles.Request(query, userId);
        var result = await repository.SearchAsync(request, cancellationToken);
        return Ok(result.Response);
    }

    [HttpGet("feed")]
    [Authorize]
    [Produces(typeof(MultipleArticles.ResponseBody))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> FeedArticles(
        [FromServices] IArticleReadRepository repository,
        [FromQuery] FeedArticle.QueryParams query,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new FeedArticle.Request(query, userId);
        var result = await repository.FeedAsync(request, cancellationToken);
        return Ok(result.Response);
    }
}
