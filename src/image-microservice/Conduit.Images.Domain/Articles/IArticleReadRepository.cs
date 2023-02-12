namespace Conduit.Images.Domain.Articles;

public interface IArticleReadRepository
{
    Task<ArticleDomainModel?> FindAsync(Guid id, CancellationToken cancellationToken);
}
