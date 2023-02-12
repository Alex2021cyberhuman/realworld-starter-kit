using Conduit.Comments.Domain.Comments.Create;

namespace Conduit.Comments.Domain.Comments.Models;

public static class CommentDomainModelExtensions
{
    public static CommentOutputModel ToCommentOutputModel(
        this CommentDomainModel commentDomainModel,
        bool following = false)
    {
        return new()
        {
            Id = commentDomainModel.Id,
            CreatedAt = commentDomainModel.CreatedAt,
            UpdatedAt = commentDomainModel.UpdatedAt,
            Body = commentDomainModel.Body,
            Author =
            {
                Username = commentDomainModel.Author.Username,
                Bio = commentDomainModel.Author.Biography,
                Image = commentDomainModel.Author.Image,
                Following = following
            }
        };
    }

    public static CommentDomainModel ToCommentDomainModel(
        this CommentCreateInputModel createCommentInputModel,
        Guid authorId,
        Guid articleId,
        string articleSlug)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Body = createCommentInputModel.Comment.Body,
            AuthorId = authorId,
            ArticleId = articleId,
            ArticleSlug = articleSlug
        };
    }
}
