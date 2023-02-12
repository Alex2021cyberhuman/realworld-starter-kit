using Conduit.Comments.Domain.Comments.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Comments.DataAccess.Comments;

public static class CommentDbModelQueryableExtensions
{
    public static IQueryable<CommentOutputModel> ReturnMultipleQuery(
        this IQueryable<CommentDbModel> query,
        Guid? userId,
        string articleSlug)
    {
        return query.IncludeInQuery(userId)
            .FilterQuery(articleSlug).ConvertQuery();
    }

    public static IQueryable<CommentDbModel> IncludeInQuery(
        this IQueryable<CommentDbModel> query,
        Guid? userId)
    {
        return query.AsSplitQuery().AsNoTracking().Include(x => x.Article)
            .Include(x => x.Author).ThenInclude(x =>
                x.Followers.Where(y => userId != null && y.Id == userId));
    }

    public static IQueryable<CommentDbModel> FilterQuery(
        this IQueryable<CommentDbModel> query,
        string articleSlug)
    {
        return query.Where(x => x.Article.Slug == articleSlug);
    }

    public static IQueryable<CommentOutputModel> ConvertQuery(
        this IQueryable<CommentDbModel> query)
    {
        return query.Select(CommentDbModelConvertExtensions
            .ConvertToCommentOutputModelExpression);
    }
}
