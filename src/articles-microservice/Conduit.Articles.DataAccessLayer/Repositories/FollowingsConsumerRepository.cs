using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Profiles;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public class FollowingsConsumerRepository : IFollowingsConsumerRepository
{
    private readonly ArticlesDbContext _articlesDbContext;

    public FollowingsConsumerRepository(
        ArticlesDbContext articlesDbContext)
    {
        _articlesDbContext = articlesDbContext;
    }

    public async Task CreateAsync(
        CreateFollowingEventModel model)
    {
        await using var transaction =
            await _articlesDbContext.Database.BeginTransactionAsync();
        var followed = await _articlesDbContext.Author
            .Include(x => x.Followers.Where(y => y.Id == model.FollowerId))
            .FirstAsync(x => x.Id == model.FollowedId);

        if (followed.Followers.Any())
        {
            throw new InvalidOperationException("Followers already added");
        }

        var follower = await _articlesDbContext.Author
            .Include(x => x.Followeds.Where(y => y.Id == model.FollowedId))
            .FirstAsync(x => x.Id == model.FollowerId);

        if (follower.Followeds.Any())
        {
            throw new InvalidOperationException("Followeds already added");
        }

        followed.Followers.Add(follower);
        follower.Followeds.Add(followed);
        await _articlesDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task RemoveAsync(
        RemoveFollowingEventModel model)
    {
        await using var transaction =
            await _articlesDbContext.Database.BeginTransactionAsync();
        var followed = await _articlesDbContext.Author
            .Include(x => x.Followers.Where(y => y.Id == model.FollowerId))
            .FirstAsync(x => x.Id == model.FollowedId);

        if (followed.Followers.Any() == false)
        {
            throw new InvalidOperationException("Followers not yet added");
        }

        followed.Followers.Remove(followed.Followers.First());
        await _articlesDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
