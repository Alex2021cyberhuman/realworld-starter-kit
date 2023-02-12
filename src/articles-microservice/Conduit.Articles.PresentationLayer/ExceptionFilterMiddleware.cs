namespace Conduit.Articles.PresentationLayer;

public static class ExceptionFilterMiddleware
{
    public static IApplicationBuilder UseExceptionFilter(
        this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionFilter>();
    }
}
