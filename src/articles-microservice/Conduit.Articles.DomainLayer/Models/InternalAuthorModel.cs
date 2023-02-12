namespace Conduit.Articles.DomainLayer.Models;

public class InternalAuthorModel
{
    public InternalAuthorModel(
        Guid id,
        string username,
        string? bio,
        string? image,
        bool following)
    {
        Id = id;
        Username = username;
        Bio = bio;
        Image = image;
        Following = following;
    }

    public Guid Id { get; set; }

    public string Username { get; set; }

    public string? Bio { get; set; }

    public string? Image { get; set; }

    public bool Following { get; set; }

    public static implicit operator AuthorModel(
        InternalAuthorModel model)
    {
        return new(model.Username, model.Bio, model.Image, model.Following);
    }
}
