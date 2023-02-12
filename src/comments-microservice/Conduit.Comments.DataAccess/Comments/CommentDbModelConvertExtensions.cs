using System.Linq.Expressions;
using Conduit.Comments.DataAccess.Authors;
using Conduit.Comments.Domain.Comments.Models;

namespace Conduit.Comments.DataAccess.Comments;

public static class CommentDbModelConvertExtensions
{
    static CommentDbModelConvertExtensions()
    {
        ConvertToCommentOutputModelExpression = dbModel => new()
        {
            Id = dbModel.Id,
            Author = new()
            {
                Bio = dbModel.Author.Bio,
                Image = dbModel.Author.Image,
                Username = dbModel.Author.Username,
                Following = dbModel.Author.Followers.Count == 1
            },
            Body = dbModel.Body,
            CreatedAt = dbModel.CreatedAt,
            UpdatedAt = dbModel.UpdatedAt
        };
    }

    /// <remarks>
    ///     <c>Following</c> property will works only if <c>AuthorDbModel.Followers</c> is filtered with
    ///     <c>Include</c>
    /// </remarks>
    public static Expression<Func<CommentDbModel, CommentOutputModel>>
        ConvertToCommentOutputModelExpression
    { get; }

    public static CommentDomainModel ToCommentDomainModel(
        this CommentDbModel dbModel)
    {
        return new()
        {
            Id = dbModel.Id,
            ArticleId = dbModel.ArticleId,
            ArticleSlug = dbModel.Article.Slug,
            Author = dbModel.Author.ToAuthorDomainModel(),
            AuthorId = dbModel.AuthorId,
            Body = dbModel.Body,
            CreatedAt = dbModel.CreatedAt,
            UpdatedAt = dbModel.UpdatedAt
        };
    }

    public static CommentDbModel ToCommentDbModel(
        this CommentDomainModel domainModel)
    {
        return new()
        {
            Id = domainModel.Id,
            ArticleId = domainModel.ArticleId,
            AuthorId = domainModel.AuthorId,
            Body = domainModel.Body,
            CreatedAt = domainModel.CreatedAt,
            UpdatedAt = domainModel.UpdatedAt
        };
    }
}
