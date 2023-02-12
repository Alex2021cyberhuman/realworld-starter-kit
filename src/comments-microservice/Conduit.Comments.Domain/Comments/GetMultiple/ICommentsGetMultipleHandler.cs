namespace Conduit.Comments.Domain.Comments.GetMultiple;

public interface ICommentsGetMultipleHandler
{
    Task<CommentsGetMultipleResponse> HandleAsync(
        CommentsGetMultipleRequest request,
        CancellationToken cancellationToken);
}
