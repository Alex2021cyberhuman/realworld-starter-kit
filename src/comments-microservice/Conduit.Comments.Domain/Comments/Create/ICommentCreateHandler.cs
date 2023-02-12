namespace Conduit.Comments.Domain.Comments.Create;

public interface ICommentCreateHandler
{
    Task<CommentCreateResponse> HandleAsync(
        CommentCreateRequest request,
        CancellationToken cancellationToken);
}
