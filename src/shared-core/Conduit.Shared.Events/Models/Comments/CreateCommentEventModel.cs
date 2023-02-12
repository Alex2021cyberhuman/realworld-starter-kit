using System.Text.Json.Serialization;

namespace Conduit.Shared.Events.Models.Comments;

public class CreateCommentEventModel
{
    [JsonPropertyName("b")]
    public string Body { get; set; } = string.Empty;

    [JsonPropertyName("ca")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("ua")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("ui")]
    public Guid UserId { get; set; }

    [JsonPropertyName("ai")]
    public Guid ArticleId { get; set; }

    [JsonPropertyName("i")]
    public Guid Id { get; set; }
}
