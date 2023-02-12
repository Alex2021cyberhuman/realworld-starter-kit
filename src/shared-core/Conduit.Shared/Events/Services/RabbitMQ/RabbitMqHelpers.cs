using System.Text.Json;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public static class RabbitMqHelpers
{
    public static JsonSerializerOptions DefaultJsonSerializerOptions =>
        new(JsonSerializerDefaults.Web);

    public static ReadOnlyMemory<byte> BytorizeAsJson<T>(
        this T item)
    {
        return BytorizeAsJson(item, DefaultJsonSerializerOptions);
    }

    public static ReadOnlyMemory<byte> BytorizeAsJson<T>(
        this T item,
        JsonSerializerOptions options)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(item, options);
        var memory = new ReadOnlyMemory<byte>(bytes);
        return memory;
    }

    public static T? DeBytorizeAsJson<T>(
        this ReadOnlyMemory<byte> memory,
        JsonSerializerOptions? options = null)
    {
        options ??= DefaultJsonSerializerOptions;
        var item = JsonSerializer.Deserialize<T>(memory.Span, options);

        return item;
    }

    public static T DeBytorizeAsRequiredJson<T>(
        this ReadOnlyMemory<byte> memory,
        JsonSerializerOptions? options = null)
    {
        var item = memory.DeBytorizeAsJson<T>(options);

        if (item is null)
        {
            throw new InvalidOperationException(
                "Error with deserialization occupied: item is null");
        }

        return item;
    }
}
