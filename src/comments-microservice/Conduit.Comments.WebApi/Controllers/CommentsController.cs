using System.Net;
using Conduit.Comments.Domain;
using Conduit.Comments.Domain.Comments.Create;
using Conduit.Comments.Domain.Comments.Delete;
using Conduit.Comments.Domain.Comments.GetMultiple;
using Conduit.Comments.Domain.Comments.Models;
using Conduit.Shared.AuthorizationExtensions;
using Conduit.Shared.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Comments.WebApi.Controllers;

[ApiController]
[Route("articles/{articleSlug}/comments")]
public class CommentsController : ControllerBase
{
    [HttpPost(Name = "createComment")]
    [ProducesResponseType(typeof(SingleCommentOutputModel),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CommentCreateInputModel body,
        [FromRoute] string articleSlug,
        [FromServices] ICommentCreateHandler handler,
        CancellationToken cancellationToken)
    {
        var authorId = HttpContext.GetCurrentUserId();
        var request = new CommentCreateRequest
        {
            ArticleSlug = articleSlug,
            AuthorId = authorId,
            Model = body
        };
        var response = await handler.HandleAsync(request, cancellationToken);
        return GetActionResult(response.Output, response.Validation,
            response.Error);
    }


    [HttpGet(Name = "getMultipleComments")]
    [ProducesResponseType(typeof(MultipleCommentsOutputModel),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetMultiple(
        [FromRoute] string articleSlug,
        [FromServices] ICommentsGetMultipleHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserIdOptional();
        var request = new CommentsGetMultipleRequest
        {
            ArticleSlug = articleSlug,
            UserId = userId
        };
        var response = await handler.HandleAsync(request, cancellationToken);
        return GetActionResult(response.Output, null, response.Error);
    }


    [HttpDelete("{commentId:guid}", Name = "deleteComment")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [Authorize]
    public async Task<IActionResult> Delete(
        [FromRoute] string articleSlug,
        [FromServices] ICommentDeleteHandler handler,
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var authorId = HttpContext.GetCurrentUserId();
        var request = new CommentDeleteRequest
        {
            ArticleSlug = articleSlug,
            AuthorId = authorId,
            CommentId = commentId
        };
        var response = await handler.HandleAsync(request, cancellationToken);
        return GetActionResult(null, null, response.Error);
    }

    private IActionResult GetActionResult(
        object? output,
        Validation? validation,
        Error error)
    {
        return error switch
        {
            Error.None => output != null ? Ok(output) : NoContent(),
            Error.BadRequest => validation.ToBadRequest(),
            Error.NotFound => NotFound(),
            Error.Forbidden => Forbid(),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}
