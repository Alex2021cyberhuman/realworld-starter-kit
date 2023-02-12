using Conduit.Images.Domain.Images.Common;

namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageReadRepository
{
    Task<ArticleImageDomainModel?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ArticleImageDomainModel>> FindByIdsAsync(HashSet<Guid> imageIds, CancellationToken cancellationToken);
}
