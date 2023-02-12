using Conduit.Comments.Domain.Articles;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Comments.DataAccess.Articles;

public class ArticleReadRepository : IArticleReadRepository
{
    private readonly CommentsContext _context;

    public ArticleReadRepository(
        CommentsContext context)
    {
        _context = context;
    }

    public async Task<ArticleDomainModel?> FindAsync(
        string articleSlug,
        CancellationToken cancellationToken)
    {
        var dbModel =
            await _context.Article.FirstOrDefaultAsync(
                x => x.Slug == articleSlug, cancellationToken);
        var domainModel = dbModel?.ToArticleDomainModel();
        return domainModel;
    }
}
