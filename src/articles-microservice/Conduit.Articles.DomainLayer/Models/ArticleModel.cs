namespace Conduit.Articles.DomainLayer.Models;

public class ArticleModel
{
    public ArticleModel(
        string slug,
        string title,
        string description,
        string body,
        ISet<string> tagList,
        DateTime createdAt,
        DateTime updatedAt,
        bool favorited,
        int favoritesCount,
        AuthorModel author)
    {
        Slug = slug;
        Title = title;
        Description = description;
        Body = body;
        TagList = tagList;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Favorited = favorited;
        FavoritesCount = favoritesCount;
        Author = author;
    }

    public ArticleModel()
    {
    }

    public string Slug { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public ISet<string> TagList { get; set; } = new HashSet<string>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool Favorited { get; set; }

    public int FavoritesCount { get; set; }

    public AuthorModel Author { get; set; } = new();
}
