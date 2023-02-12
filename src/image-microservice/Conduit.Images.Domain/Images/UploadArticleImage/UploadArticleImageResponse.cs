using Conduit.Images.Domain.Images.Common;
using Conduit.Shared.ResponsesExtensions;

namespace Conduit.Images.Domain.Images.UploadArticleImage;

public class UploadArticleImageResponse : BaseResponse
{
    public UploadArticleImageResponse(Model data)
    {
        Data = data;
    }

    public Model? Data { get; }

    public class Model
    {

        public Model(ArticleImageResponseModel image)
        {
            Image = image;
        }

        public ArticleImageResponseModel Image { get; }
    }
}
