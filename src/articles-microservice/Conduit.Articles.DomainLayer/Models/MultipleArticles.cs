namespace Conduit.Articles.DomainLayer.Models;

public class MultipleArticles
{
    public MultipleArticles(
        IEnumerable<ArticleModel> articles,
        int articlesCount)
    {
        Response = new(articles, articlesCount);
    }

    public ResponseBody Response { get; set; }

    public class ResponseBody
    {
        public ResponseBody(
            IEnumerable<ArticleModel> articles,
            int articlesCount)
        {
            Articles = articles;
            ArticlesCount = articlesCount;
        }

        public IEnumerable<ArticleModel> Articles { get; set; }

        public int ArticlesCount { get; set; }
    }
}
