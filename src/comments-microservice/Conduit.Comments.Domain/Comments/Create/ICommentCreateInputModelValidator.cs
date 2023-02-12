using Conduit.Shared.Validation;

namespace Conduit.Comments.Domain.Comments.Create;

public interface ICommentCreateInputModelValidator
{
    Task<Validation> ValidateAsync(
        CommentCreateInputModel commentCreateInputModel);
}
