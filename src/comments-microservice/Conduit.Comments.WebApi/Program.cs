using System.Globalization;
using Conduit.Comments.BusinessLogic.Articles;
using Conduit.Comments.BusinessLogic.Authors;
using Conduit.Comments.BusinessLogic.Comments.Create;
using Conduit.Comments.BusinessLogic.Comments.Delete;
using Conduit.Comments.BusinessLogic.Comments.GetMultiple;
using Conduit.Comments.DataAccess;
using Conduit.Comments.DataAccess.Articles;
using Conduit.Comments.DataAccess.Authors;
using Conduit.Comments.DataAccess.Comments;
using Conduit.Comments.Domain.Articles;
using Conduit.Comments.Domain.Authors;
using Conduit.Comments.Domain.Comments.Create;
using Conduit.Comments.Domain.Comments.Delete;
using Conduit.Comments.Domain.Comments.GetMultiple;
using Conduit.Comments.Domain.Comments.Repositories;
using Conduit.Comments.WebApi;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Conduit.Shared.Events.Models.Comments.CreateComment;
using Conduit.Shared.Events.Models.Comments.DeleteComment;
using Conduit.Shared.Events.Models.Profiles.CreateFollowing;
using Conduit.Shared.Events.Models.Profiles.RemoveFollowing;
using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Models.Users.Update;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Localization;
using Conduit.Shared.Startup;
using Conduit.Shared.Tokens;
using Conduit.Shared.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region ServicesConfiguration

var environment = builder.Environment;
var configuration = builder.Configuration;

var logging = builder.Logging;
logging.ClearProviders();
var serilogLogger = new LoggerConfiguration().ReadFrom
    .Configuration(configuration).CreateLogger();
logging.AddSerilog(serilogLogger);

var services = builder.Services;
var supportedCultures = new CultureInfo[] { new("ru"), new("en") };
services.AddControllers().Localize<SharedResource>(supportedCultures);
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new() { Title = "Conduit.Comments.WebApi", Version = "v1" });
});

services.AddJwtServices(configuration.GetSection("Jwt").Bind)
    .DisableDefaultModelValidation().AddDbContext<CommentsContext>(
        optionsBuilder =>
        {
            if (environment.IsDevelopment())
            {
                optionsBuilder.EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            }

            optionsBuilder.UseSnakeCaseNamingConvention().UseNpgsql(
                configuration.GetConnectionString("Comments"),
                contextOptionsBuilder => contextOptionsBuilder
                    .UseQuerySplittingBehavior(
                        QuerySplittingBehavior.SplitQuery));
        }).AddScoped<ICommentCreateHandler, CommentCreateHandler>()
    .AddScoped<ICommentCreateInputModelValidator,
        CommentCreateInputModelValidator>()
    .AddScoped<ICommentDeleteHandler, CommentDeleteHandler>()
    .AddScoped<ICommentsGetMultipleHandler, CommentsGetMultipleHandler>()
    .AddScoped<IArticleConsumerRepository, ArticleConsumerRepository>()
    .AddScoped<IArticleReadRepository, ArticleReadRepository>()
    .AddScoped<IAuthorConsumerRepository, AuthorConsumerRepository>()
    .AddScoped<ICommentsReadRepository, CommentReadRepository>()
    .AddScoped<ICommentsWriteRepository, CommentWriteRepository>()
    .AddW3CLogging(configuration.GetSection("W3C").Bind).AddHttpClient()
    .AddHttpContextAccessor()
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind)
    .AddHealthChecks().AddDbContextCheck<CommentsContext>().Services
    .RegisterConsumer<RegisterUserEventModel,
        RegisterUserEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<UpdateUserEventModel,
        UpdateUserEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<CreateArticleEventModel,
        CreateArticleEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<UpdateArticleEventModel,
        UpdateArticleEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<DeleteArticleEventModel,
        DeleteArticleEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<CreateFollowingEventModel,
        CreateFollowingEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<RemoveFollowingEventModel,
        RemoveFollowingEventConsumer>(ConfigureConsumer)
    .RegisterProducer<CreateCommentEventModel>()
    .RegisterProducer<DeleteCommentEventModel>();

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
await initializationScope.InitializeDatabase();
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();

static void ConfigureConsumer<T>(
    RabbitMqSettings<T> options)
{
    options.Consumer = "comments";
}
