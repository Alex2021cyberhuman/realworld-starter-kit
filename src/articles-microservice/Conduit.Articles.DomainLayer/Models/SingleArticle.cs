namespace Conduit.Articles.DomainLayer.Models;

public class SingleArticle
{
    public SingleArticle(
        ArticleModel article)
    {
        Response = new(article);
    }

    public ResponseBody Response { get; set; }

    public class ResponseBody
    {
        public ResponseBody(
            ArticleModel article)
        {
            Article = article;
        }

        public ArticleModel Article { get; set; }
    }
}
