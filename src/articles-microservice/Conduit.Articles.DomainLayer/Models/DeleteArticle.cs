namespace Conduit.Articles.DomainLayer.Models;

public static class DeleteArticle
{
    public class Request
    {
        public Request(
            Guid currentUserId,
            string slug)
        {
            CurrentUserId = currentUserId;
            Slug = slug;
        }

        public Guid CurrentUserId { get; set; }
        public string Slug { get; set; }
    }
}
