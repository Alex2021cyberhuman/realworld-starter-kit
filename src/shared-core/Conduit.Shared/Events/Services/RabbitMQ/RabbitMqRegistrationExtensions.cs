using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public static class RabbitMqRegistrationExtensions
{
    public static async Task InitializeQueuesAsync(
        this IServiceScope scope)
    {
        var rabbitMqInitializer = scope.ServiceProvider
            .GetRequiredService<RabbitMqInitializer>();
        await rabbitMqInitializer.StartAsync();
    }

    public static IServiceCollection RegisterRabbitMqWithHealthCheck(
        this IServiceCollection services,
        Action<ConnectionFactory> configureFactory)
    {
        var connectionFactory = new ConnectionFactory
        {
            DispatchConsumersAsync = true
        };
        configureFactory(connectionFactory);
        return services.AddSingleton(connectionFactory)
            .AddScoped<RabbitMqInitializer>().AddHealthChecks()
            .AddRabbitMQ(_ => connectionFactory).Services;
    }

    public static IServiceCollection RegisterProducer<T>(
        this IServiceCollection services,
        Action<RabbitMqSettings<T>>? configureAction = null)
    {
        var settings = GetSettings(configureAction);
        return services.AddSingleton<IRabbitMqSettings>(settings)
            .AddSingleton(settings)
            .AddSingleton<IEventProducer<T>, SimpleRabbitMqProducer<T>>();
    }

    public static IServiceCollection RegisterConsumer<T, TEventConsumer>(
        this IServiceCollection services,
        Action<RabbitMqSettings<T>>? configureAction = null)
        where TEventConsumer : class, IEventConsumer<T>
    {
        var settings = GetSettings(configureAction);
        settings.AsConsumer = true;
        return services.AddSingleton<IRabbitMqSettings>(settings)
            .AddSingleton(settings)
            .AddScoped<IEventConsumer<T>, TEventConsumer>()
            .AddHostedService<SimpleRabbitMqConsumer<T>>();
    }

    private static RabbitMqSettings<T> GetSettings<T>(
        Action<RabbitMqSettings<T>>? configureAction)
    {
        var settings = new RabbitMqSettings<T>();
        configureAction?.Invoke(settings);
        return settings;
    }
}
