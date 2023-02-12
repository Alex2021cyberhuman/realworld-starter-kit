using System.Globalization;
using Conduit.Images.BusinessLogic.Articles;
using Conduit.Images.BusinessLogic.Extensions;
using Conduit.Images.DataAccess.Extensions;
using Conduit.Images.Domain.Configuration;
using Conduit.Images.WebApi;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Localization;
using Conduit.Shared.Startup;
using Conduit.Shared.Tokens;
using Conduit.Shared.Validation;
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
        new() { Title = "Conduit.Images.WebApi", Version = "v1" });
});

services.AddJwtServices(configuration.GetSection("Jwt").Bind)
    .DisableDefaultModelValidation()
    .AddW3CLogging(configuration.GetSection("W3C").Bind).AddHttpClient()
    .AddHttpContextAccessor()
    .Configure<ImageConfiguration>(builder.Configuration.GetSection("ImageConfiguration").Bind)
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind)
    .AddHealthChecks().Services
    .RegisterConsumer<CreateArticleEventModel,
        CreateArticleEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<UpdateArticleEventModel,
        UpdateArticleEventConsumer>(ConfigureConsumer)
    .RegisterConsumer<DeleteArticleEventModel,
        DeleteArticleEventConsumer>(ConfigureConsumer);

var migrationsIsOn =  builder.Configuration.GetSection("Migrations").Get<bool>();
services.RegisterDataAccessLayer(builder.Configuration, migrationsIsOn);
services.RegisterBusinessLogicLayer();

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
await initializationScope.InitializeDataAccessLayerAsync(migrationsIsOn);
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();

static void ConfigureConsumer<T>(
    RabbitMqSettings<T> options)
{
    options.Consumer = "images";
}
