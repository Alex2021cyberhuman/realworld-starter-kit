using Conduit.Comments.Domain.Authors;

namespace Conduit.Comments.Domain.Comments.Models;

public class CommentDomainModel
{
    public Guid Id { get; set; }

    public string ArticleSlug { get; set; } = string.Empty;

    public Guid ArticleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Body { get; set; } = string.Empty;

    public Guid AuthorId { get; set; }

    public AuthorDomainModel Author { get; set; } = new();
}
