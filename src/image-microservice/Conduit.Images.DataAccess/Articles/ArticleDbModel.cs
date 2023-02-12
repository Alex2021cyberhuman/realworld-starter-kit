using Dapper.FluentMap.Mapping;

namespace Conduit.Images.DataAccess.Articles;

public class ArticleDbModel
{
    public Guid Id { get; init; }

    public string Slug { get; init; } = string.Empty;

    public Guid AuthorId { get; init; }

    public class EntityMap : EntityMap<ArticleDbModel>
    {
        public EntityMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Slug).ToColumn("slug");
            Map(p => p.AuthorId).ToColumn("author_id");
        }
    }
}
