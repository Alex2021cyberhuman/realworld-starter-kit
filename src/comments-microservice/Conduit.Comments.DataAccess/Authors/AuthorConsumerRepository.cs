using Conduit.Comments.Domain.Authors;
using Conduit.Shared.Events.Models.Profiles.CreateFollowing;
using Conduit.Shared.Events.Models.Profiles.RemoveFollowing;
using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Models.Users.Update;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Comments.DataAccess.Authors;

public class AuthorConsumerRepository : IAuthorConsumerRepository
{
    private readonly CommentsContext _context;

    public AuthorConsumerRepository(
        CommentsContext context)
    {
        _context = context;
    }

    public async Task RegisterAsync(
        RegisterUserEventModel eventModel)
    {
        var dbModel = new AuthorDbModel
        {
            Id = eventModel.Id,
            Username = eventModel.Username,
            Image = eventModel.Image,
            Bio = eventModel.Biography
        };
        _context.Author.Add(dbModel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        UpdateUserEventModel eventModel)
    {
        var dbModel =
            await _context.Author.FirstAsync(x => x.Id == eventModel.Id);
        dbModel.Image = eventModel.Image;
        dbModel.Bio = eventModel.Biography;
        dbModel.Username = eventModel.Username;
        _context.Author.Update(dbModel);
        await _context.SaveChangesAsync();
    }


    public async Task CreateFollowingAsync(
        CreateFollowingEventModel eventModel)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();
        var followedAuthorDbModel = await _context.Author
            .Include(x => x.Followers.Where(y => y.Id == eventModel.FollowerId))
            .FirstAsync(x => x.Id == eventModel.FollowedId);

        if (followedAuthorDbModel.Followers.Any())
        {
            throw new InvalidOperationException("Followers already added");
        }

        var followerAuthorDbModel = await _context.Author
            .Include(x => x.Followeds.Where(y => y.Id == eventModel.FollowedId))
            .FirstAsync(x => x.Id == eventModel.FollowerId);

        if (followerAuthorDbModel.Followeds.Any())
        {
            throw new InvalidOperationException("Followeds already added");
        }

        followedAuthorDbModel.Followers.Add(followerAuthorDbModel);
        followerAuthorDbModel.Followeds.Add(followedAuthorDbModel);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task RemoveFollowingAsync(
        RemoveFollowingEventModel eventModel)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();
        var followedAuthorDbModel = await _context.Author
            .Include(x => x.Followers.Where(y => y.Id == eventModel.FollowerId))
            .FirstAsync(x => x.Id == eventModel.FollowedId);

        if (followedAuthorDbModel.Followers.Any() == false)
        {
            throw new InvalidOperationException("Followers not yet added");
        }

        followedAuthorDbModel.Followers.Remove(followedAuthorDbModel.Followers
            .First());
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
