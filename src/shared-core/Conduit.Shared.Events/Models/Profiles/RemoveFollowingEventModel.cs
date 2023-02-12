using System.Text.Json.Serialization;

namespace Conduit.Shared.Events.Models.Profiles;

public record RemoveFollowingEventModel
{
    [JsonPropertyName("fr")]
    public Guid FollowerId { get; set; }

    [JsonPropertyName("fd")]
    public Guid FollowedId { get; set; }
}
