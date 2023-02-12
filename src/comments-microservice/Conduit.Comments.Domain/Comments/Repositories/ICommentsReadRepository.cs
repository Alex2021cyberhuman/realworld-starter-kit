using Conduit.Comments.Domain.Comments.Models;

namespace Conduit.Comments.Domain.Comments.Repositories;

public interface ICommentsReadRepository
{
    Task<CommentDomainModel?> FindAsync(
        Guid commentId,
        CancellationToken cancellationToken);

    Task<List<CommentOutputModel>> GetMultipleAsync(
        string articleSlug,
        Guid? userId,
        CancellationToken cancellationToken);
}
