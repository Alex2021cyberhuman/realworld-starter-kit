using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Images.Domain.Images.UploadArticleImage;

namespace Conduit.Images.BusinessLogic.Images.UploadArticleImage;

public class UploadArticleImageRequestHandler : IUploadArticleImageRequestHandler
{
    private readonly IImageWriteRepository _imageWriteRepository;
    private readonly IImageStorageNameGenerator _imageStorageNameGenerator;
    private readonly IImageUrlProvider _imageUrlProvider;

    public UploadArticleImageRequestHandler(
        IImageWriteRepository imageWriteRepository,
        IImageStorageNameGenerator imageStorageNameGenerator,
        IImageUrlProvider imageUrlProvider)
    {
        _imageWriteRepository = imageWriteRepository;
        _imageStorageNameGenerator = imageStorageNameGenerator;
        _imageUrlProvider = imageUrlProvider;
    }

    public async Task<UploadArticleImageResponse> UploadAsync(UploadArticleImageRequest request, CancellationToken cancellationToken = default)
    {
        var imageId = Guid.NewGuid();
        var userId = request.UserId;
        var mediaType = request.ContentType;
        var uploaded = DateTime.UtcNow;
        var imageStorageName = _imageStorageNameGenerator.Generate(userId, imageId, mediaType);
        var stream = await request.StreamProvider.ProvideStreamAsync();
        var imageUrl = _imageUrlProvider.Provide(imageStorageName);
        var domainModel = await _imageWriteRepository.SaveAsync(
            userId,
            stream,
            mediaType,
            imageId,
            imageStorageName,
            imageUrl,
            uploaded,
            cancellationToken);

        var uploadArticleImageResponse = new UploadArticleImageResponse(
            new(new(
                domainModel.ArticleId,
                domainModel.Id,
                domainModel.Url,
                domainModel.MediaType)));
        return uploadArticleImageResponse;
    }
}
