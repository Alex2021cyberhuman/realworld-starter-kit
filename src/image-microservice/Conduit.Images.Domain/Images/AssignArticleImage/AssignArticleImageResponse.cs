using Conduit.Shared.ResponsesExtensions;

namespace Conduit.Images.Domain.Images.AssignArticleImage;

public class AssignArticleImageResponse : BaseResponse
{
    public AssignArticleImageResponse(Error error,
        string? errorDescription = null) : base(error,
        errorDescription)
    {
    }
}
