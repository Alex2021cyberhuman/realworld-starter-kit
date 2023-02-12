using Conduit.Comments.Domain.Comments.Models;

namespace Conduit.Comments.Domain.Comments.GetMultiple;

public class CommentsGetMultipleResponse : BaseResponse
{
    public CommentsGetMultipleResponse(
        Error error)
    {
        Error = error;
    }

    public CommentsGetMultipleResponse(
        List<CommentOutputModel> items)
    {
        Output = new() { Comments = items };
    }

    public MultipleCommentsOutputModel Output { get; set; } = new();
}
