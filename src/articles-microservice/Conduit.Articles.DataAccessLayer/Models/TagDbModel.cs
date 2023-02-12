namespace Conduit.Articles.DataAccessLayer.Models;

public class TagDbModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<ArticleDbModel> Articles { get; set; } = null!;
}
