using Conduit.Comments.Domain.Authors;

namespace Conduit.Comments.DataAccess.Authors;

public static class AuthorDbModelConvertExtensions
{
    public static AuthorDomainModel ToAuthorDomainModel(
        this AuthorDbModel dbModel)
    {
        return new()
        {
            Id = dbModel.Id,
            Biography = dbModel.Bio,
            Image = dbModel.Image,
            Username = dbModel.Username
        };
    }
}
