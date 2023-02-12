using Conduit.Comments.DataAccess.Articles;
using Conduit.Comments.DataAccess.Authors;

namespace Conduit.Comments.DataAccess.Comments;

public class CommentDbModel
{
    public Guid Id { get; set; }

    public Guid ArticleId { get; set; }

    public ArticleDbModel Article { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public AuthorDbModel Author { get; set; } = null!;

    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
