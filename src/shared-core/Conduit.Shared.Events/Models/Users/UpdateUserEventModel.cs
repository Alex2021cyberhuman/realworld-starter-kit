using System.Text.Json.Serialization;

namespace Conduit.Shared.Events.Models.Users;

public record UpdateUserEventModel
{
    [JsonPropertyName("i")]
    public Guid Id { get; init; }

    [JsonPropertyName("n")]
    public string Username { get; init; } = string.Empty;

    [JsonPropertyName("e")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("im")]
    public string? Image { get; init; }

    [JsonPropertyName("b")]
    public string? Biography { get; init; }
}
