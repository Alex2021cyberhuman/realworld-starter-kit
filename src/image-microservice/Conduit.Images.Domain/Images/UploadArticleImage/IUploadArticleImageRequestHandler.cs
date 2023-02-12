namespace Conduit.Images.Domain.Images.UploadArticleImage;

public interface IUploadArticleImageRequestHandler
{
    Task<UploadArticleImageResponse> UploadAsync(UploadArticleImageRequest request, CancellationToken cancellationToken = default);
}
