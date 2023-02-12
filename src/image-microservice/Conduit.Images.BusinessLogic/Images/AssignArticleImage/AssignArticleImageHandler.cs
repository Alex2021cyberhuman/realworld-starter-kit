using Conduit.Images.BusinessLogic.Images.Shared;
using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.AssignArticleImage;
using Conduit.Images.Domain.Images.Common;
using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Shared.ResponsesExtensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Conduit.Images.BusinessLogic.Images.AssignArticleImage;

public class AssignArticleImageHandler : IAssignArticleImageHandler
{
    private readonly IImageReadRepository _imageReadRepository;

    private readonly IImageWriteRepository _imageWriteRepository;

    private readonly IArticleReadRepository _articleReadRepository;

    private readonly ILogger _logger;

    private readonly IStringLocalizer _stringLocalizer;

    public AssignArticleImageHandler(
        IImageReadRepository imageReadRepository,
        IImageWriteRepository imageWriteRepository,
        IArticleReadRepository articleReadRepository,
        ILogger<AssignArticleImageHandler> logger,
        IStringLocalizer stringLocalizer)
    {
        _imageReadRepository = imageReadRepository;
        _imageWriteRepository = imageWriteRepository;
        _articleReadRepository = articleReadRepository;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }


    public async Task<AssignArticleImageResponse> AssignAsync(AssignArticleImageRequest assignArticleImageRequest, CancellationToken cancellationToken = default)
    {
        var articleId = assignArticleImageRequest.ArticleId;
        var imageIds = assignArticleImageRequest.Body.ImagesToAssign;
        var userId = assignArticleImageRequest.UserId;
        _logger.LogInformation("Begin AsssignArticleImage {ArticleId} {UserId} {ImageIds}", articleId, userId, imageIds);

        var article = await _articleReadRepository.FindAsync(articleId, cancellationToken);

        var images = await _imageReadRepository.FindByIdsAsync(imageIds, cancellationToken);

        var validationResult = Validate(article,
            articleId,
            userId,
            images,
            imageIds);

        if (validationResult != null)
        {
            return validationResult;
        }

        var assignableImageIds = images
            .Where(x => x.ArticleId != articleId)
            .Select(x => x.Id).ToHashSet();
        await _imageWriteRepository.AssignAsync(
            article!,
            assignableImageIds,
            CancellationToken.None);
        _logger.LogInformation("End AssignArticleImage successful");
        return new(Error.None);
    }

    public AssignArticleImageResponse? Validate(
        ArticleDomainModel? article,
        Guid articleId,
        Guid userId,
        IReadOnlyCollection<ArticleImageDomainModel> images,
        IReadOnlyCollection<Guid> imageIds)
    {
        if (article is null)
        {
            _logger.LogWarning("End AsssignArticleImage article not found {ArticleId}", articleId);
            var errorDescription = _stringLocalizer[LocalizationKeys.ArticleNotFound];
            return new(Error.NotFound, errorDescription);
        }

        if (article.AuthorId != userId)
        {
            _logger.LogWarning("End AsssignArticleImage {UserId} user is not an author of article {ArticleId}", userId, articleId);
            var errorDescription = _stringLocalizer[LocalizationKeys.UserNotAuthor];
            return new(Error.Forbidden, errorDescription);
        }

        if (images.Count != imageIds.Count)
        {
            _logger.LogWarning("End AsssignArticleImage images not found");
            var errorDescription = _stringLocalizer[LocalizationKeys.SomeImagesNotFound];
            return new(Error.NotFound, errorDescription);
        }

        if (images.Any(x => x.UserId != userId))
        {
            _logger.LogWarning("End AsssignArticleImage images not uploaded by user {UserId}", userId);
            var errorDescription = _stringLocalizer[LocalizationKeys.SomeImagesNotUploadedByUser];
            return new(Error.Forbidden, errorDescription);
        }

        if (images.Any(x => x.ArticleId != null))
        {
            _logger.LogWarning("End AsssignArticleImage some images already assigned", userId);
            var errorDescription = _stringLocalizer[LocalizationKeys.SomeImagesAlreadyAssigned];
            return new(Error.Forbidden, errorDescription);
        }

        return null;
    }
}
