namespace Conduit.Images.Domain.Articles;

public class ArticleDomainModel
{
    public Guid Id { get; set; }

    public string Slug { get; set; } = string.Empty;

    public Guid AuthorId { get; set; }
}
