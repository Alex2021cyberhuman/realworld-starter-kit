namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageUrlProvider
{
    public string Provide(string storageName);
}
