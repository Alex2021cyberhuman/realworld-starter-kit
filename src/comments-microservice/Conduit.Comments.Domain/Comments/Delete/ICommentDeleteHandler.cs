namespace Conduit.Comments.Domain.Comments.Delete;

public interface ICommentDeleteHandler
{
    Task<CommentDeleteResponse> HandleAsync(
        CommentDeleteRequest request,
        CancellationToken cancellationToken);
}
