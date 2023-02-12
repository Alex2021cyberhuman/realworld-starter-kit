using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.Common;

namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageWriteRepository
{
    public Task<ArticleImageDomainModel> SaveAsync(
        Guid userId,
        Stream stream,
        string mediaType,
        Guid imageId,
        string storageName,
        string imageUrl,
        DateTime uploaded,
        CancellationToken cancellationToken = default);

    public Task RemoveUnassignedOlderThanAsync(DateTime dateTime, CancellationToken cancellationToken = default);

    public Task RemoveAsync(ArticleImageDomainModel articleImageDomainModel, CancellationToken cancellationToken = default);

    public Task AssignAsync(ArticleDomainModel article, ISet<Guid> imageIds, CancellationToken cancellationToken = default);
}
