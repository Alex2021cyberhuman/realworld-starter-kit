using Conduit.Images.Domain.Images.RemoveArticleImage;
using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Shared.ResponsesExtensions;

namespace Conduit.Images.BusinessLogic.Images.RemoveArticleImage;

public class RemoveArticleImageHandler : IRemoveArticleImageHandler
{
    private readonly IImageWriteRepository _imageWriteRepository;

    private readonly IImageReadRepository _imageReadRepository;
    public RemoveArticleImageHandler(
        IImageReadRepository imageReadRepository,
        IImageWriteRepository imageWriteRepository)
    {
        _imageReadRepository = imageReadRepository;
        _imageWriteRepository = imageWriteRepository;
    }

    public async Task<RemoveArticleImageResponse> RemoveAsync(RemoveArticleImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = await _imageReadRepository.FindAsync(request.ImageId, cancellationToken);
        if (image is null)
        {
            return new(Error.NotFound);
        }

        var canUserRemoveImage = image.UserId == request.UserId;
        if (!canUserRemoveImage)
        {
            return new(Error.Forbidden);
        }

        await _imageWriteRepository.RemoveAsync(image, CancellationToken.None);
        return new(Error.None);
    }
}
