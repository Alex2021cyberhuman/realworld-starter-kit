namespace Conduit.Comments.Domain.Comments.Models;

public class SingleCommentOutputModel
{
    public CommentOutputModel Comment { get; set; } = new();
}
