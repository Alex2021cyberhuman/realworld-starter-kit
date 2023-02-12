using Conduit.Articles.DomainLayer.Models;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface ITagRepository
{
    Task<TagList.Response> GetTags(
        CancellationToken cancellationToken);
}
