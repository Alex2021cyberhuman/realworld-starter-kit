using Conduit.Images.Domain.Images.Bases;

namespace Conduit.Images.Domain.Images.Common;

public class ArticleImageResponseModel : ImageResponseModel
{
    public ArticleImageResponseModel(
        Guid? articleId,
        Guid id,
        string url,
        string mediaType) : base(id, url, mediaType)
    {
        ArticleId = articleId;
    }

    public Guid? ArticleId { get; }
}


public abstract class ImageDomainModel
{
    public ImageDomainModel(
        Guid id,
        Guid userId,
        string url,
        string storageName,
        string mediaType,
        DateTime uploaded)
    {
        Id = id;
        Url = url;
        StorageName = storageName;
        MediaType = mediaType;
        Uploaded = uploaded;
        UserId = userId;
    }

    public Guid Id { get; }

    public Guid UserId { get; set; }

    public string Url { get; }

    public string StorageName { get; set; }

    public string MediaType { get; }

    public DateTime Uploaded { get; set; }
}
public class ArticleImageDomainModel : ImageDomainModel
{
    public ArticleImageDomainModel(
        Guid id,
        Guid userId,
        string url,
        string storageName,
        string mediaType,
        DateTime uploaded,
        Guid? articleId = null) : base(id, userId, url, storageName, mediaType, uploaded)
    {
        ArticleId = articleId;
    }

    /// <summary>
    ///     ArticleId
    /// </summary>
    /// <value>Null if not yet assigned</value>
    public Guid? ArticleId { get; }
}
