using Conduit.Images.Domain.Configuration;
using Conduit.Images.Domain.Images.RemoveUnassignedImages;
using Conduit.Images.Domain.Images.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Conduit.Images.BusinessLogic.Images.RemoveUnassignedImages;

public class RemoveUnassignedImagesHandler : IRemoveUnassignedImagesHandler
{
    private readonly IOptionsMonitor<ImageConfiguration> _imageConfigurationOptionsMonitor;
    private readonly IImageWriteRepository _imageWriteRepository;

    public RemoveUnassignedImagesHandler(
        IOptionsMonitor<ImageConfiguration> imageConfigurationOptionsMonitor,
        IImageWriteRepository imageWriteRepository)
    {
        _imageConfigurationOptionsMonitor = imageConfigurationOptionsMonitor;
        _imageWriteRepository = imageWriteRepository;
    }

    private ImageConfiguration ImageConfigurationInstance => _imageConfigurationOptionsMonitor.CurrentValue;

    public async Task RemoveAsync(CancellationToken cancellationToken = default)
    {
        var interval = ImageConfigurationInstance.UnassignedImagesRemovingTime;
        var now = DateTime.UtcNow;
        var deleteLine = now - interval;
        await _imageWriteRepository.RemoveUnassignedOlderThanAsync(deleteLine, cancellationToken);
    }
}
