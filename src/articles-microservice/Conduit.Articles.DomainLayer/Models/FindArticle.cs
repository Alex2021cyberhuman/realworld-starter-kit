using System.ComponentModel.DataAnnotations;

namespace Conduit.Articles.DomainLayer.Models;

public static class FindArticle
{
    public class Request
    {
        public Request(
            Guid? currentUserId,
            string slug)
        {
            CurrentUserId = currentUserId;
            Slug = slug;
        }

        [Required]
        public string Slug { get; set; }

        public Guid? CurrentUserId { get; set; }
    }
}
