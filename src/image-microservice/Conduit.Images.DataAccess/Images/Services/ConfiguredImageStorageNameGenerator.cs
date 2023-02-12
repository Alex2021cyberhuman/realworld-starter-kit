using Conduit.Images.Domain.Configuration;
using Conduit.Images.Domain.Images.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Conduit.Images.DataAccess.Images.Services;

public class ConfiguredImageStorageNameGenerator : IImageStorageNameGenerator
{
    private readonly IOptionsMonitor<Options> _optionsMonitor;

    private readonly IOptionsMonitor<ImageConfiguration> _imageConfigurationsMonitor;

    public ConfiguredImageStorageNameGenerator(
        IOptionsMonitor<Options> optionsMonitor,
        IOptionsMonitor<ImageConfiguration> imageConfigurationsMonitor)
    {
        _optionsMonitor = optionsMonitor;
        _imageConfigurationsMonitor = imageConfigurationsMonitor;
    }

    private Options OptionsInstance => _optionsMonitor.CurrentValue;

    private ImageConfiguration ImageConfigurationsInstance => _imageConfigurationsMonitor.CurrentValue;

    public string Generate(Guid userId, Guid imageId, string mediaType)
    {
        var mediaTypeMapping = ImageConfigurationsInstance.MediaTypeMapping;
        var extension = mediaTypeMapping[mediaType];
        var storageNameFormat = OptionsInstance.StorageNameFormat;
        var storageName = string.Format(storageNameFormat, userId, imageId, extension);
        return storageName;
    }

    public class Options
    {
        public string StorageNameFormat { get; set; } = "usercontent-{0}-image-{1}.{2}";
    }
}
