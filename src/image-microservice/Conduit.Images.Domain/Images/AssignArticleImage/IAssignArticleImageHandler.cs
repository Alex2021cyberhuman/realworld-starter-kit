namespace Conduit.Images.Domain.Images.AssignArticleImage;

public interface IAssignArticleImageHandler
{
    Task<AssignArticleImageResponse> AssignAsync(AssignArticleImageRequest assignArticleImageRequest, CancellationToken cancellationToken = default);
}
