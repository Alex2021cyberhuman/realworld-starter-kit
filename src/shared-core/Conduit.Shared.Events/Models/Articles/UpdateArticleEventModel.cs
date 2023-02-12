using System.Text.Json.Serialization;

namespace Conduit.Shared.Events.Models.Articles;

public class UpdateArticleEventModel
{
    [JsonPropertyName("i")]
    public Guid Id { get; set; }

    [JsonPropertyName("s")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("t")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("d")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("b")]
    public string Body { get; set; } = string.Empty;

    [JsonPropertyName("tl")]
    public ISet<string> TagList { get; set; } = new HashSet<string>();

    [JsonPropertyName("ca")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("ua")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("ui")]
    public Guid AuthorId { get; set; }
}
