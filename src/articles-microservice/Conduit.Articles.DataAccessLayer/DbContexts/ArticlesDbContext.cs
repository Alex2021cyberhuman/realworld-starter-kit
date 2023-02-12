using System.Reflection;
using Conduit.Articles.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.DbContexts;

public class ArticlesDbContext : DbContext
{
    protected ArticlesDbContext()
    {
    }

    public ArticlesDbContext(
        DbContextOptions<ArticlesDbContext> options) : base(options)
    {
    }

    public DbSet<TagDbModel> Tag => Set<TagDbModel>();

    public DbSet<ArticleDbModel> Article => Set<ArticleDbModel>();

    public DbSet<AuthorDbModel> Author => Set<AuthorDbModel>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}
