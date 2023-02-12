using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DataAccessLayer.Utilities;
using Conduit.Articles.DomainLayer.Exceptions;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public class ArticleReadRepository : IArticleReadRepository
{
    private readonly ArticlesDbContext _context;

    public ArticleReadRepository(
        ArticlesDbContext context)
    {
        _context = context;
    }

    public async Task<SingleArticle> FindAsync(
        FindArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var article = await _context.Article.Include(x => x.Tags)
            .Include(x => x.Author)
            .ThenInclude(x =>
                x.Followers.Where(follower =>
                    follower.Id == request.CurrentUserId))
            .Include(x =>
                x.Favoriters.Where(favoriter =>
                    favoriter.Id == request.CurrentUserId))
            .FirstOrDefaultAsync(x => x.Slug == request.Slug,
                cancellationToken);

        if (article is null)
        {
            throw new NotFoundException();
        }

        Console.WriteLine(string.Join(",", article.Author.Followers));
        Console.WriteLine(string.Join(",", article.Favoriters));
        var singleArticle =
            article.MapArticleToSingleArticle(article.Author.Followers.Any(),
                article.Favoriters.Any());

        return singleArticle;
    }

    public async Task<MultipleArticles> SearchAsync(
        SearchArticles.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Article.IncludeArticles();
        query = query.FilterArticles(request);
        return await ReturnMultipleArticles(query, request.Query.Offset,
            request.Query.Limit, request.CurrentUserId, cancellationToken);
    }

    public async Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Article.IncludeArticles();
        query = query.Where(x =>
            x.Author.Followers.Any(y => y.Id == request.CurrentUserId));
        return await ReturnMultipleArticles(query, request.Query.Offset,
            request.Query.Limit, request.CurrentUserId, cancellationToken);
    }

    private static async Task<MultipleArticles> ReturnMultipleArticles(
        IQueryable<ArticleDbModel> query,
        int queryOffset,
        int queryLimit,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        var articleCount = await query.CountAsync(cancellationToken);
        var articles = await query
            .SelectArticles(queryOffset, queryLimit, userId)
            .ToListAsync(cancellationToken);
        var result = new MultipleArticles(articles, articleCount);
        return result;
    }
}
