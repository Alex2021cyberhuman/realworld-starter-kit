using Conduit.Comments.Domain.Comments.Models;
using Conduit.Shared.Validation;

namespace Conduit.Comments.Domain.Comments.Create;

public class CommentCreateResponse : BaseResponse
{
    public CommentCreateResponse(
        Validation validation)
    {
        Validation = validation;
        Error = Error.BadRequest;
    }

    public CommentCreateResponse()
    {
    }

    public CommentCreateResponse(
        Error error)
    {
        Error = error;
    }

    public CommentCreateResponse(
        CommentOutputModel comment)
    {
        Output = new() { Comment = comment };
    }

    public Validation? Validation { get; set; }

    public SingleCommentOutputModel Output { get; set; } = new();
}
