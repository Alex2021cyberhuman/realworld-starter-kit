using Conduit.Images.Domain.Articles;

namespace Conduit.Images.DataAccess.Articles;

public static class ArticleDbModelExtensions
{
    public static ArticleDomainModel? ToArticleDomainModel(this ArticleDbModel? model) => model is null ? null : new()
    {
        Id = model.Id,
        Slug = model.Slug,
        AuthorId = model.AuthorId
    };
}
