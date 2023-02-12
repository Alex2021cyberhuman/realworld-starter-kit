using System.Globalization;
using Conduit.Likes.BusinessLogic.Articles;
using Conduit.Likes.DataAccess;
using Conduit.Likes.DataAccess.Articles;
using Conduit.Likes.DataAccess.Favorites;
using Conduit.Likes.Domain.Articles;
using Conduit.Likes.Domain.Favorites;
using Conduit.Likes.Domain.Favorites.FavoriteArticle;
using Conduit.Likes.Domain.Favorites.UnfavoriteArticle;
using Conduit.Likes.WebApi;
using Conduit.Shared.Events.Models.Articles;
using Conduit.Shared.Events.Models.Likes;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Localization;
using Conduit.Shared.Startup;
using Conduit.Shared.Tokens;
using Conduit.Shared.Validation;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

#region ServicesConfiguration

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;
var supportedCultures = new CultureInfo[] { new("ru"), new("en") };
services.AddControllers().Localize<SharedResource>(supportedCultures);
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new() { Title = "Conduit.Likes.WebApi", Version = "v1" });
});

services.AddJwtServices(configuration.GetSection("Jwt").Bind)
    .DisableDefaultModelValidation()
    .AddW3CLogging(configuration.GetSection("W3C").Bind).AddHttpClient()
    .AddHttpContextAccessor()
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind)
    .AddHealthChecks().AddRedis(GetRedisConnectionString(configuration))
    .Services
    .RegisterConsumer<CreateArticleEventModel,
        CreateArticleConsumer>(ConfigureConsumer)
    .RegisterConsumer<UpdateArticleEventModel,
        UpdateArticleConsumer>(ConfigureConsumer)
    .RegisterConsumer<DeleteArticleEventModel,
        DeleteArticleConsumer>(ConfigureConsumer)
    .RegisterProducer<FavoriteArticleEventModel>()
    .RegisterProducer<UnfavoriteArticleEventModel>()
    .AddSingleton<FavoriteArticleHandler,
        Conduit.Likes.BusinessLogic.Favorites.FavoriteArticle.
        FavoriteArticleHandler>()
    .AddSingleton<UnfavoriteArticleHandler,
        Conduit.Likes.BusinessLogic.Favorites.UnfavoriteArticle.
        UnfavoriteArticleHandler>()
    .AddSingleton<IFavoritesRepository, FavoritesRepository>()
    .AddSingleton<IArticleRepository, ArticlesRepository>()
    .AddSingleton<IArticleConsumerRepository, ArticleConsumerRepository>()
    .AddSingleton<ConnectionProvider>()
    .Configure<ConnectionProviderOptions>(configuration
        .GetSection("RedisDatabase").Bind);

#endregion

var app = builder.Build();

#region AppConfiguration

if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    IdentityModelEventSource.ShowPII = true;
}

app.UseRouting();
app.UseCors(options =>
    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseW3CLogging();
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var initializationScope = app.Services.CreateScope();

await initializationScope.WaitHealthyServicesAsync(TimeSpan.FromHours(1));
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();

static void ConfigureConsumer<T>(
    RabbitMqSettings<T> settings)
{
    settings.Consumer = "likes";
}

static string GetRedisConnectionString(
    IConfiguration configuration)
{
    var redisConnectionProviderOptions = new ConnectionProviderOptions();
    configuration.GetSection("RedisDatabase")
        .Bind(redisConnectionProviderOptions);
    return redisConnectionProviderOptions.Configuration;
}
