using Conduit.Images.Domain.Images.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Conduit.Images.DataAccess.Images.Services;

public class ConfiguredImageUrlProvider : IImageUrlProvider
{
    private readonly IOptionsMonitor<Options> _optionsMonitor;

    public ConfiguredImageUrlProvider(IOptionsMonitor<Options> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    private Options OptionsInstance => _optionsMonitor.CurrentValue;

    public string Provide(string storageName)
    {
        var enpointToRetriveFormat = OptionsInstance.EnpointToRetriveFormat;
        var url = string.Format(enpointToRetriveFormat, storageName);
        return url;
    }

    public class Options
    {
        public string EnpointToRetriveFormat { get; set; } = "http://localhost/images/{0}";
    }
}
