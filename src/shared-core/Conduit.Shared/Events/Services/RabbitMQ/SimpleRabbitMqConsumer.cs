using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public class SimpleRabbitMqConsumer<T> : BaseRabbitMqEventConsumer<T>
{
    public SimpleRabbitMqConsumer(
        ConnectionFactory connectionFactory,
        ILoggerFactory loggerFactory,
        IServiceScopeFactory serviceScopeFactory,
        RabbitMqSettings<T> settings) : base(connectionFactory, loggerFactory,
        serviceScopeFactory, settings)
    {
    }
}
