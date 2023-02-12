using Conduit.Images.Domain.Images.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Conduit.Images.DataAccess.Images.Services;

public class LocalImageStorage : IImageStorage
{
    public IOptionsMonitor<Options> _optionsMonitor;

    public LocalImageStorage(IOptionsMonitor<Options> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public Options OptionsInstance => _optionsMonitor.CurrentValue;

    public Task RemoveAsync(string storageName, CancellationToken cancellationToken = default)
    {
        RemoveSync(storageName);
        return Task.CompletedTask;
    }

    public Task MultipleRemoveAsync(IReadOnlyCollection<string> storageNames, CancellationToken cancellationToken = default)
    {
        storageNames.AsParallel().ForAll(RemoveSync);
        return Task.CompletedTask;
    }

    private void RemoveSync(string storageName)
    {
        var localDirectory = OptionsInstance.LocalDirectory;
        var fullFileName = Path.Combine(localDirectory, storageName);
        var isExists = File.Exists(fullFileName);
        if (isExists)
        {
            File.Delete(fullFileName);
        }
    }

    public Task<Stream?> RetrieveAsync(string storageName, CancellationToken cancellationToken = default)
    {
        var localDirectory = OptionsInstance.LocalDirectory;
        var fullFileName = Path.Combine(localDirectory, storageName);
        var isExists = File.Exists(fullFileName);
        if (isExists)
        {
            return Task.FromResult((Stream?)File.OpenRead(fullFileName));
        }
        return Task.FromResult((Stream?)null);
    }

    public async Task StoreAsync(string storageName, Stream stream, CancellationToken cancellationToken = default)
    {
        var localDirectory = OptionsInstance.LocalDirectory;
        var fullFileName = Path.Combine(localDirectory, storageName);
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        using var fileStream = File.OpenWrite(fullFileName);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }

    public class Options
    {
        public string LocalDirectory { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
    }
}
