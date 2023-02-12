using Conduit.Comments.Domain.Articles;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Comments.DataAccess.Articles;

public class ArticleConsumerRepository : IArticleConsumerRepository
{
    private readonly CommentsContext _context;

    public ArticleConsumerRepository(
        CommentsContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(
        CreateArticleEventModel eventModel)
    {
        var dbModel = new ArticleDbModel
        {
            Id = eventModel.Id,
            Slug = eventModel.Slug,
            AuthorId = eventModel.AuthorId
        };
        _context.Add(dbModel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        UpdateArticleEventModel eventModel)
    {
        var dbModel =
            await _context.Article.FirstAsync(x => x.Id == eventModel.Id);
        dbModel.Slug = eventModel.Slug;
        dbModel.AuthorId = eventModel.AuthorId;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(
        DeleteArticleEventModel eventModel)
    {
        var dbModel =
            await _context.Article.FirstAsync(x => x.Id == eventModel.Id);
        _context.Article.Remove(dbModel);
        await _context.SaveChangesAsync();
    }
}
