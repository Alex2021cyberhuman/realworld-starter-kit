using Conduit.Comments.Domain.Articles;

namespace Conduit.Comments.DataAccess.Articles;

public static class ArticleDbModelConvertExtensions
{
    public static ArticleDomainModel ToArticleDomainModel(
        this ArticleDbModel dbModel)
    {
        return new()
        {
            Id = dbModel.Id,
            Slug = dbModel.Slug,
            AuthorId = dbModel.AuthorId
        };
    }
}
