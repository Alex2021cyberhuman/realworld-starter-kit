using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public class SimpleRabbitMqProducer<T> : BaseRabbitMqEventProducer<T>
{
    public SimpleRabbitMqProducer(
        ConnectionFactory connectionFactory,
        RabbitMqSettings<T> settings,
        ILogger<SimpleRabbitMqProducer<T>> logger) : base(connectionFactory,
        settings, logger)

    {
    }
}
