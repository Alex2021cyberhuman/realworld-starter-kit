namespace Conduit.Articles.DomainLayer.Models;

public class AuthorModel
{
    public AuthorModel(
        string username,
        string? bio,
        string? image,
        bool following)
    {
        Username = username;
        Bio = bio;
        Image = image;
        Following = following;
    }

    public AuthorModel()
    {
    }

    public string Username { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? Image { get; set; }

    public bool Following { get; set; }
}
