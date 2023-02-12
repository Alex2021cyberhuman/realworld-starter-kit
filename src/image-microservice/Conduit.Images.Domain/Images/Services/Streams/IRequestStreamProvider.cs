namespace Conduit.Images.Domain.Images.Services.Streams;

public interface IRequestStreamProvider
{
    Task<Stream> ProvideStreamAsync();
}
