namespace Conduit.Comments.Domain.Comments.GetMultiple;

public class CommentsGetMultipleRequest
{
    public Guid? UserId { get; set; }

    public string ArticleSlug { get; set; } = string.Empty;
}
