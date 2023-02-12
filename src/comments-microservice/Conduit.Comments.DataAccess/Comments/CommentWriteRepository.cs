using Conduit.Comments.Domain.Comments.Models;
using Conduit.Comments.Domain.Comments.Repositories;

namespace Conduit.Comments.DataAccess.Comments;

public class CommentWriteRepository : ICommentsWriteRepository
{
    private readonly CommentsContext _context;

    public CommentWriteRepository(
        CommentsContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(
        CommentDomainModel commentDomainModel)
    {
        var dbModel = commentDomainModel.ToCommentDbModel();
        _context.Add(dbModel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(
        Guid commentId)
    {
        var dbModel = await _context.Comment.FindAsync(commentId);
        _context.Comment.Remove(dbModel!);
        await _context.SaveChangesAsync();
    }
}
