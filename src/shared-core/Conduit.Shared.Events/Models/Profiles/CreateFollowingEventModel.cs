using System.Text.Json.Serialization;

namespace Conduit.Shared.Events.Models.Profiles;

public class CreateFollowingEventModel
{
    [JsonPropertyName("fr")]
    public Guid FollowerId { get; set; }

    [JsonPropertyName("fd")]
    public Guid FollowedId { get; set; }
}
