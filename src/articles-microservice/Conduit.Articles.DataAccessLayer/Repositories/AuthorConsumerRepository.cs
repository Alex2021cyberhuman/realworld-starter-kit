using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Users;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public class AuthorConsumerRepository : IAuthorConsumerRepository
{
    private readonly ArticlesDbContext _articlesDbContext;

    public AuthorConsumerRepository(
        ArticlesDbContext articlesDbContext)
    {
        _articlesDbContext = articlesDbContext;
    }

    public async Task RegisterAsync(
        RegisterUserEventModel model)
    {
        var authorDbModel = new AuthorDbModel
        {
            Id = model.Id,
            Username = model.Username,
            Image = model.Image,
            Bio = model.Biography
        };
        _articlesDbContext.Author.Add(authorDbModel);
        await _articlesDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        UpdateUserEventModel model)
    {
        var authorDbModel = new AuthorDbModel
        {
            Id = model.Id,
            Username = model.Username,
            Image = model.Image,
            Bio = model.Biography
        };
        _articlesDbContext.Author.Update(authorDbModel);
        await _articlesDbContext.SaveChangesAsync();
    }
}
