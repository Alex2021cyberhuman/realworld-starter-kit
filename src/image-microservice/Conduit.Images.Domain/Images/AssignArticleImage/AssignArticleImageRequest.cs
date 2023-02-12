namespace Conduit.Images.Domain.Images.AssignArticleImage;

public class AssignArticleImageRequest
{
    public AssignArticleImageRequest(Guid userId, Guid articleId, RequestBody body)
    {
        UserId = userId;
        ArticleId = articleId;
        Body = body;
    }

    public Guid UserId { get; set; }

    public Guid ArticleId { get; set; }

    public RequestBody Body { get; set; }

    public class RequestBody
    {
        public HashSet<Guid> ImagesToAssign { get; set; } = new();
    }
}
