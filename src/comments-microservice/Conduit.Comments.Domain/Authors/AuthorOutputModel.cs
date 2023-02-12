using System.ComponentModel.DataAnnotations;

namespace Conduit.Comments.Domain.Authors;

public class AuthorOutputModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? Image { get; set; }

    [Required]
    public bool Following { get; set; }
}
