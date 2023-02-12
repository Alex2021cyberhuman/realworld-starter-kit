using Conduit.Comments.Domain.Comments.Models;

namespace Conduit.Comments.Domain.Comments.Repositories;

public interface ICommentsWriteRepository
{
    Task CreateAsync(
        CommentDomainModel commentDomainModel);

    Task DeleteAsync(
        Guid commentId);
}
