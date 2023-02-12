namespace Conduit.Images.Domain.Images.RemoveArticleImage;

public class RemoveArticleImageRequest
{
    public RemoveArticleImageRequest(Guid userId, Guid imageId)
    {
        UserId = userId;
        ImageId = imageId;
    }

    public Guid UserId { get; set; }

    public Guid ImageId { get; set; }
}
