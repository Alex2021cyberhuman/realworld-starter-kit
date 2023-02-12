using RabbitMQ.Client;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public interface IRabbitMqSettings
{
    void Initialize(
        IModel channel);
}
