namespace Conduit.Comments.DataAccess.Authors;

public class AuthorDbModel
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? Image { get; set; }

    public ICollection<AuthorDbModel> Followers { get; set; } = null!;

    public ICollection<AuthorDbModel> Followeds { get; set; } = null!;
}
