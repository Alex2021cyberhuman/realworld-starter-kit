using Conduit.Comments.Domain;
using Conduit.Comments.Domain.Articles;
using Conduit.Comments.Domain.Comments.Delete;
using Conduit.Comments.Domain.Comments.Models;
using Conduit.Comments.Domain.Comments.Repositories;
using Conduit.Shared.Events.Models.Comments.DeleteComment;
using Conduit.Shared.Events.Services;
using Microsoft.Extensions.Logging;

namespace Conduit.Comments.BusinessLogic.Comments.Delete;

public class CommentDeleteHandler : ICommentDeleteHandler
{
    private readonly IArticleReadRepository _articleReadRepository;
    private readonly ICommentsReadRepository _commentsReadRepository;
    private readonly ICommentsWriteRepository _commentsWriteRepository;

    private readonly IEventProducer<DeleteCommentEventModel>
        _deleteCommentEventModelEventProducer;

    private readonly ILogger<CommentDeleteHandler> _logger;

    public CommentDeleteHandler(
        IArticleReadRepository articleReadRepository,
        ICommentsWriteRepository commentsWriteRepository,
        ICommentsReadRepository commentsReadRepository,
        IEventProducer<DeleteCommentEventModel>
            deleteCommentEventModelEventProducer,
        ILogger<CommentDeleteHandler> logger)
    {
        _articleReadRepository = articleReadRepository;
        _commentsWriteRepository = commentsWriteRepository;
        _commentsReadRepository = commentsReadRepository;
        _deleteCommentEventModelEventProducer =
            deleteCommentEventModelEventProducer;
        _logger = logger;
    }

    public async Task<CommentDeleteResponse> HandleAsync(
        CommentDeleteRequest request,
        CancellationToken cancellationToken)
    {
        var articleDomainModel =
            await _articleReadRepository.FindAsync(request.ArticleSlug,
                cancellationToken);
        if (articleDomainModel is null)
        {
            _logger.LogWarning(
                "Cannot create comment article not found {Request}", request);
            return new(Error.NotFound);
        }

        var comment =
            await _commentsReadRepository.FindAsync(request.CommentId,
                cancellationToken);
        if (comment is null)
        {
            _logger.LogWarning("Cannot delete comment not found {Request}",
                request);
            return new(Error.NotFound);
        }

        var canUserDeleteComment = CheckCanUserDeleteComment(comment, request);
        if (canUserDeleteComment == false)
        {
            _logger.LogWarning("Cannot delete comment forbidden {Request}",
                request);
            return new(Error.Forbidden);
        }

        await _commentsWriteRepository.DeleteAsync(request.CommentId);
        var deleteCommentEventModel = new DeleteCommentEventModel
        {
            Id = comment.Id,
            ArticleId = comment.ArticleId,
            UserId = request.AuthorId
        };

        await _deleteCommentEventModelEventProducer.ProduceEventAsync(
            deleteCommentEventModel);
        _logger.LogInformation("Comment deleted {Request}", request);

        return new();
    }

    private static bool CheckCanUserDeleteComment(
        CommentDomainModel comment,
        CommentDeleteRequest request)
    {
        return comment.AuthorId == request.AuthorId;
    }
}
