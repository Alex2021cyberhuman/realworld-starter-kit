namespace Conduit.Images.Domain.Images.RemoveUnassignedImages;

public interface IRemoveUnassignedImagesHandler
{
    Task RemoveAsync(CancellationToken cancellationToken = default);
}

