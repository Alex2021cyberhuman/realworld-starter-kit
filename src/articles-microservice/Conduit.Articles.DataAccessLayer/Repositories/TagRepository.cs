using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ArticlesDbContext _articlesDbContext;

    public TagRepository(
        ArticlesDbContext articlesDbContext)
    {
        _articlesDbContext = articlesDbContext;
    }

    public async Task<TagList.Response> GetTags(
        CancellationToken cancellationToken)
    {
        var list = await _articlesDbContext.Tag.AsNoTracking()
            .Select(x => x.Name).ToListAsync(cancellationToken);
        var response = new TagList.Response(new() { Tags = list });
        return response;
    }
}
