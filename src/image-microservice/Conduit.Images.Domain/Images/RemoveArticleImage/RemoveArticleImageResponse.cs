using Conduit.Shared.ResponsesExtensions;

namespace Conduit.Images.Domain.Images.RemoveArticleImage;

public class RemoveArticleImageResponse : BaseResponse
{
    public RemoveArticleImageResponse(Error error): base(error)
    {
    }
}
