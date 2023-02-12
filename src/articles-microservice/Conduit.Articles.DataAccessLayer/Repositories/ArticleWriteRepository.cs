using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DataAccessLayer.Utilities;
using Conduit.Articles.DomainLayer.Exceptions;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Articles.DomainLayer.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public class ArticleWriteRepository : IArticleWriteRepository
{
    private readonly ArticlesDbContext _context;
    private readonly ISlugilizator _slugilizator;

    public ArticleWriteRepository(
        ArticlesDbContext context,
        ISlugilizator slugilizator)
    {
        _context = context;
        _slugilizator = slugilizator;
    }

    public async Task<InternalArticleModel> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);
        var model = article.Body.Article;
        var author = await _context.Author.FindAsync(
            new object[] { article.CurrentUserId }, cancellationToken);
        var tags = await GetTagsAsync(model.TagList, cancellationToken);

        var articleDbModel = new ArticleDbModel
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AuthorId = article.CurrentUserId,
            Author = author!,
            Body = model.Body,
            Description = model.Description,
            Title = model.Title,
            Slug = _slugilizator.GetSlug(model.Title),
            Tags = tags
        };

        _context.Add(articleDbModel);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return articleDbModel.MapArticleToInternalArticleModel();
    }

    public async Task<InternalArticleModel> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);
        var articleDbModel =
            await FindArticleDbModelAsync(article.Slug, cancellationToken);
        CheckAccess(article.CurrentUserId, articleDbModel);
        var model = article.Body.Article;

        articleDbModel.UpdatedAt = DateTime.UtcNow;
        articleDbModel.AuthorId = article.CurrentUserId;
        articleDbModel.Body = model.Body ?? articleDbModel.Body;
        articleDbModel.Description =
            model.Description ?? articleDbModel.Description;
        articleDbModel.Title = model.Title ?? articleDbModel.Title;
        articleDbModel.Slug = UpdateSlug(model, articleDbModel);
        articleDbModel.Tags =
            await UpdateTagsAsync(model, articleDbModel, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return articleDbModel.MapArticleToInternalArticleModel();
    }

    public async Task<InternalArticleModel> DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);
        var removedArticle =
            await FindArticleDbModelAsync(article.Slug, cancellationToken);
        CheckAccess(article.CurrentUserId, removedArticle);
        _context.Remove(removedArticle);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return removedArticle.MapArticleToInternalArticleModel();
    }

    private static void CheckAccess(
        Guid userId,
        ArticleDbModel articleDbModel)
    {
        if (articleDbModel.Author.Id != userId)
        {
            throw new ForbiddenException();
        }
    }

    private async Task<ArticleDbModel> FindArticleDbModelAsync(
        string articleSlug,
        CancellationToken cancellationToken)
    {
        var item = await _context.Article.Include(x => x.Tags)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Slug == articleSlug, cancellationToken);

        if (item is null)
        {
            throw new NotFoundException();
        }

        return item;
    }

    private string UpdateSlug(
        UpdateArticle.Model model,
        ArticleDbModel old)
    {
        return model.Title is not null && model.Title != old.Title
            ? _slugilizator.GetSlug(model.Title)
            : old.Slug;
    }

    private async Task<ICollection<TagDbModel>> UpdateTagsAsync(
        UpdateArticle.Model model,
        ArticleDbModel old,
        CancellationToken cancellationToken)
    {
        var tags = model.TagList is null
            ? old.Tags
            : await GetTagsAsync(model.TagList, cancellationToken);
        return tags;
    }

    private async Task<List<TagDbModel>> GetTagsAsync(
        IEnumerable<string> tagList,
        CancellationToken cancellationToken)
    {
        var tagListClone = tagList.ToHashSet();
        var tags = await _context.Tag.Where(x => tagListClone.Contains(x.Name))
            .ToListAsync(cancellationToken);
        tags.ForEach(x => tagListClone.Remove(x.Name));
        tags.AddRange(tagListClone.Select(x => new TagDbModel { Name = x }));
        return tags;
    }
}
