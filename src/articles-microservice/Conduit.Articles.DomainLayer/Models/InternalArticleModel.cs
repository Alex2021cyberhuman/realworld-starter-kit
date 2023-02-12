namespace Conduit.Articles.DomainLayer.Models;

public class InternalArticleModel
{
    public InternalArticleModel(
        Guid id,
        string slug,
        string title,
        string description,
        string body,
        HashSet<string> hashSet,
        DateTime createdAt,
        DateTime updatedAt,
        bool favorited,
        int favoritesCount,
        InternalAuthorModel author)
    {
        Id = id;
        Slug = slug;
        Title = title;
        Description = description;
        Body = body;
        HashSet = hashSet;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Favorited = favorited;
        FavoritesCount = favoritesCount;
        Author = author;
    }

    public Guid Id { get; set; }

    public string Slug { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Body { get; set; }

    public ISet<string> TagList { get; set; } = new HashSet<string>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool Favorited { get; set; }

    public int FavoritesCount { get; set; }

    public InternalAuthorModel Author { get; set; }
    public HashSet<string> HashSet { get; }

    public static implicit operator ArticleModel(
        InternalArticleModel model)
    {
        return new(model.Slug, model.Title, model.Description, model.Body,
            model.TagList, model.CreatedAt, model.UpdatedAt, model.Favorited,
            model.FavoritesCount, model.Author);
    }
}
