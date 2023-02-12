namespace Conduit.Images.Domain.Images.RemoveArticleImage;

public interface IRemoveArticleImageHandler
{
    Task<RemoveArticleImageResponse> RemoveAsync(RemoveArticleImageRequest request, CancellationToken cancellationToken = default);
}
