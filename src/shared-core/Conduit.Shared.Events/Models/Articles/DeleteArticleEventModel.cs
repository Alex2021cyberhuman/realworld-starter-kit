using System.Text.Json.Serialization;

namespace Conduit.Shared.Events.Models.Articles;

public class DeleteArticleEventModel
{
    [JsonPropertyName("i")]
    public Guid Id { get; set; }

    [JsonPropertyName("ui")]
    public Guid UserId { get; set; }
}
