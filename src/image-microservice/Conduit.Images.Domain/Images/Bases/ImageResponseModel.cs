namespace Conduit.Images.Domain.Images.Bases;

public abstract class ImageResponseModel
{
    public ImageResponseModel(
        Guid id,
        string url,
        string mediaType)
    {
        this.Id = id;
        this.Url = url;
        this.MediaType = mediaType;
    }

    public Guid Id { get; }


    public string Url { get; }


    public string MediaType { get; }
}
