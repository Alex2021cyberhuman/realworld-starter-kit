namespace Conduit.Comments.Domain.Authors;

public class AuthorDomainModel
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? Biography { get; set; }

    public string? Image { get; set; }
}
