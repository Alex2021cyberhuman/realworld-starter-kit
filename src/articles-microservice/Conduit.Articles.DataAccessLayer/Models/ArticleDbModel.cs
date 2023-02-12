namespace Conduit.Articles.DataAccessLayer.Models;

public class ArticleDbModel
{
    public Guid Id { get; set; }

    public string Slug { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public ICollection<TagDbModel> Tags { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int FavoritesCount { get; set; }

    public Guid AuthorId { get; set; }

    public AuthorDbModel Author { get; set; } = null!;

    public ICollection<AuthorDbModel> Favoriters { get; set; } = null!;
}
