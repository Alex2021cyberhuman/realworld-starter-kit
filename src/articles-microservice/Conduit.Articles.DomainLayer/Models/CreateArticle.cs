using System.ComponentModel.DataAnnotations;
using Conduit.Shared.Validation;

namespace Conduit.Articles.DomainLayer.Models;

public static class CreateArticle
{
    public class RequestBody
    {
        [NestedValidation]
        public Model Article { get; set; } = new();
    }

    public class Request
    {
        public Request(
            RequestBody body,
            Guid currentUserId)
        {
            Body = body;
            CurrentUserId = currentUserId;
        }

        [NestedValidation]
        public RequestBody Body { get; set; }

        public Guid CurrentUserId { get; set; }
    }

    public class Model
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        [Required]
        public HashSet<string> TagList { get; set; } = new();
    }
}
