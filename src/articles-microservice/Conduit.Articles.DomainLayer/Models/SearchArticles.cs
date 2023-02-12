using System.ComponentModel;

namespace Conduit.Articles.DomainLayer.Models;

public static class SearchArticles
{
    public class Request
    {
        public Request(
            QueryParams query,
            Guid? currentUserId)
        {
            Query = query;
            CurrentUserId = currentUserId;
        }

        public QueryParams Query { get; set; }

        public Guid? CurrentUserId { get; set; }
    }

    public class QueryParams
    {
        public string? Tag { get; set; }

        public string? Author { get; set; }

        public string? Favorited { get; set; }

        [DefaultValue(20)]
        public int Limit { get; set; } = 20;

        [DefaultValue(0)]
        public int Offset { get; set; } = 0;
    }
}
