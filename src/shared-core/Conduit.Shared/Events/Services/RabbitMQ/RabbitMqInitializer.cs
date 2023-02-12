using RabbitMQ.Client;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public class RabbitMqInitializer
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IEnumerable<IRabbitMqSettings> _settingsEnumerable;

    public RabbitMqInitializer(
        ConnectionFactory connectionFactory,
        IEnumerable<IRabbitMqSettings> settingsEnumerable)
    {
        _connectionFactory = connectionFactory;
        _settingsEnumerable = settingsEnumerable;
    }

    public Task StartAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        foreach (var initializer in _settingsEnumerable)
        {
            initializer.Initialize(channel);
        }

        return Task.CompletedTask;
    }
}
