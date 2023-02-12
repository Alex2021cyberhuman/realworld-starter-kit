namespace Conduit.Images.Domain.Configuration;

public class ImageConfiguration
{
    /// <summary>
    ///     Max image size
    /// </summary>
    /// <value>bytes</value>
    public long MaxImageSize { get; set; } = 1_000_000;


    /// <summary>
    ///     Accepted media types for images and its mappings to extensions
    /// </summary>
    /// <value>mediaTypes to extensions</value>
    public Dictionary<string, string> MediaTypeMapping { get; set; } = new()
    {
        ["image/apng"] = ".apng",
        ["image/avif"] = ".avif",
        ["image/gif"] = ".gif",
        ["image/jpeg"] = ".jpg",
        ["image/png"] = ".png",
        ["image/svg+xml"] = ".svg",
        ["image/webp"] = ".webp",
        ["image/bmp"] = ".bmp",
        ["image/x-icon"] = ".ico",
        ["image/tiff"] = ".tif"
    };

    public TimeSpan UnassignedImagesRemovingTime { get; set; } = TimeSpan.FromMinutes(1);
}
