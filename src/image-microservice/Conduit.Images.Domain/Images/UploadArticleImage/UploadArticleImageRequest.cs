using Conduit.Images.Domain.Images.Services.Streams;

namespace Conduit.Images.Domain.Images.UploadArticleImage;

public class UploadArticleImageRequest
{
    public UploadArticleImageRequest(
        IRequestStreamProvider streamProvider,
        string contentType,
        Guid userId)
    {
        StreamProvider = streamProvider;
        ContentType = contentType;
        UserId = userId;
    }

    public IRequestStreamProvider StreamProvider { get; }

    public string ContentType { get; set; }

    public Guid UserId { get; }
}
