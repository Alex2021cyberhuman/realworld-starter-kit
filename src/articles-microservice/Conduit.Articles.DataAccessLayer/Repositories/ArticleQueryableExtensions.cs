using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public static class ArticleQueryableExtensions
{
    public static IQueryable<ArticleDbModel> IncludeArticles(
        this IQueryable<ArticleDbModel> source)
    {
        return source.Include(x => x.Tags).Include(x => x.Author);
    }

    public static IQueryable<ArticleModel> SelectArticles(
        this IQueryable<ArticleDbModel> source,
        int queryOffset,
        int queryLimit,
        Guid? userId)
    {
        return source.Select(x => new ArticleModel
        {
            Slug = x.Slug,
            Title = x.Title,
            Description = x.Description,
            Body = x.Body,
            TagList =
                x.Tags.Select(y => y.Name).OrderBy(y => y).ToHashSet(),
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Favorited =
                userId.HasValue && x.Favoriters.Any(y => y.Id == userId),
            FavoritesCount = x.FavoritesCount,
            Author = new()
            {
                Username = x.Author.Username,
                Bio = x.Author.Bio,
                Following = userId.HasValue &&
                            x.Author.Followers.Any(y => y.Id == userId)
            }
        }).OrderBy(x => x.CreatedAt).Skip(queryOffset).Take(queryLimit);
    }

    public static IQueryable<ArticleDbModel> FilterArticles(
        this IQueryable<ArticleDbModel> source,
        SearchArticles.Request request)
    {
        return source.Where(x =>
            (request.Query.Author == null ||
             x.Author.Username == request.Query.Author) &&
            (request.Query.Tag == null ||
             x.Tags.Select(y => y.Name).Contains(request.Query.Tag)) &&
            (request.Query.Favorited == null ||
             x.Favoriters.Any(y => y.Username == request.Query.Favorited)));
    }
}
