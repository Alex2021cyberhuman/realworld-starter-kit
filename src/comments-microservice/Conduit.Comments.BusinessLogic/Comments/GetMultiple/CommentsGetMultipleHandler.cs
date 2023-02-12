using Conduit.Comments.Domain.Comments.GetMultiple;
using Conduit.Comments.Domain.Comments.Repositories;

namespace Conduit.Comments.BusinessLogic.Comments.GetMultiple;

public class CommentsGetMultipleHandler : ICommentsGetMultipleHandler
{
    private readonly ICommentsReadRepository _commentsReadRepository;

    public CommentsGetMultipleHandler(
        ICommentsReadRepository commentsReadRepository)
    {
        _commentsReadRepository = commentsReadRepository;
    }

    public async Task<CommentsGetMultipleResponse> HandleAsync(
        CommentsGetMultipleRequest request,
        CancellationToken cancellationToken)
    {
        var items =
            await _commentsReadRepository.GetMultipleAsync(request.ArticleSlug,
                request.UserId, cancellationToken);
        return new(items);
    }
}
