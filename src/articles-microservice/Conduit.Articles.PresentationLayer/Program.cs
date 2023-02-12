using System.Globalization;
using Conduit.Articles.BusinessLogicLayer;
using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Repositories;
using Conduit.Articles.DataAccessLayer.Utilities;
using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Articles.DomainLayer.Utilities;
using Conduit.Articles.PresentationLayer;
using Conduit.Shared.Events.Models.Articles;
using Conduit.Shared.Events.Models.Likes;
using Conduit.Shared.Events.Models.Profiles;
using Conduit.Shared.Events.Models.Users;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Localization;
using Conduit.Shared.Startup;
using Conduit.Shared.Tokens;
using Conduit.Shared.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

#region ServicesConfiguration

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;
var supportedCultures = new List<CultureInfo> { new("ru"), new("en") };

services.AddControllers().Localize<SharedResource>(supportedCultures);
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new() { Title = "Conduit.Articles.PresentationLayer", Version = "v1" });
});


services.AddJwtServices(configuration.GetSection("Jwt").Bind)
    .DisableDefaultModelValidation()
    .AddDbContext<ArticlesDbContext>(
        optionsBuilder =>
        {
            if (environment.IsDevelopment())
            {
                optionsBuilder.EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            }

            optionsBuilder.UseSnakeCaseNamingConvention().UseNpgsql(
                configuration.GetConnectionString("Articles"),
                contextOptionsBuilder => contextOptionsBuilder
                    .UseQuerySplittingBehavior(
                        QuerySplittingBehavior.SplitQuery));
        }).AddScoped<IArticleCreator, ArticleCreator>()
    .AddScoped<IArticleDeleter, ArticleDeleter>()
    .AddScoped<IArticleReadRepository, ArticleReadRepository>()
    .AddScoped<IArticleUpdater, ArticleUpdater>()
    .AddScoped<IArticleWriteRepository, ArticleWriteRepository>()
    .AddScoped<IAuthorConsumerRepository, AuthorConsumerRepository>()
    .AddScoped<IFavoritesConsumerRepository, FavoritesConsumerRepository>()
    .AddScoped<IFollowingsConsumerRepository, FollowingsConsumerRepository>()
    .AddScoped<ITagRepository, TagRepository>()
    .AddScoped<ISlugilizator, Slugilizator>()
    .AddScoped<IValidator<UpdateArticle.Request>,
        UpdateArticleRequestValidator>()
    .AddScoped<IValidator<CreateArticle.Request>,
        CreateArticleRequestValidator>()
    .AddW3CLogging(configuration.GetSection("W3C").Bind).AddHttpClient()
    .AddHttpContextAccessor()
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind)
    .RegisterProducer<CreateArticleEventModel>()
    .RegisterProducer<DeleteArticleEventModel>()
    .RegisterProducer<UpdateArticleEventModel>()
    .RegisterConsumer<CreateFollowingEventModel,
        CreateFollowingEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<RemoveFollowingEventModel,
        RemoveFollowingEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<UnfavoriteArticleEventModel,
        UnfavoriteArticleEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<RegisterUserEventModel,
        RegisterUserEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<FavoriteArticleEventModel,
        FavoriteArticleEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<UpdateUserEventModel,
        UpdateUserEventConsumer>(ConfigureConsumer)
    .AddSingleton<ExceptionFilter>().AddHealthChecks()
    .AddDbContextCheck<ArticlesDbContext>();

#endregion

var app = builder.Build();

#region AppConfiguration

if (environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseExceptionFilter();
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
await initializationScope.InitializeDatabase();
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();

static void ConfigureConsumer<T>(
    RabbitMqSettings<T> options)
{
    options.Consumer = "articles";
}
