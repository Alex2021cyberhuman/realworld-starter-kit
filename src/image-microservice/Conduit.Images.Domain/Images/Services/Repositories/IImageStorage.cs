namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageStorage
{
    public Task RemoveAsync(string storageName, CancellationToken cancellationToken = default);

    public Task MultipleRemoveAsync(IReadOnlyCollection<string> storageNames, CancellationToken cancellationToken = default);

    public Task StoreAsync(string storageName, Stream stream, CancellationToken cancellationToken = default);

    public Task<Stream?> RetrieveAsync(string storageName, CancellationToken cancellationToken = default);
}
