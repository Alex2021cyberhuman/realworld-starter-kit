using System.Reflection;
using RabbitMQ.Client;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public class RabbitMqSettings<T> : IRabbitMqSettings
{
    private static readonly string LowerName = typeof(T).Name.ToLower();

    public RabbitMqSettings()
    {
        Exchange = $"exchange-{LowerName}";
        RoutingKey = $"routing-{LowerName}";
        ExchangeType = "direct";
        Durable = true;
        Exclusive = false;
        AutoDelete = false;
    }

    public string Exchange { get; set; }

    public string? Consumer { get; set; }

    public bool AsConsumer { get; set; }

    public string? Queue { get; set; }

    public string RoutingKey { get; set; }

    public string ExchangeType { get; set; }

    public bool Durable { get; set; }

    public bool Exclusive { get; set; }

    public bool AutoDelete { get; set; }

    public void Initialize(
        IModel channel)
    {
        channel.ExchangeDeclare(Exchange, ExchangeType, Durable, AutoDelete,
            null);

        if (AsConsumer)
        {
            InitConsumer(channel);
        }
    }

    private void InitConsumer(
        IModel channel)
    {
        CoalesceConsumer();

        CoalesceQueue();

        _ = channel.QueueDeclare(Queue, Durable, Exclusive, AutoDelete, null);
        channel.QueueBind(Queue, Exchange, RoutingKey, null);
    }

    private void CoalesceConsumer()
    {
        if (string.IsNullOrWhiteSpace(Consumer))
        {
            Consumer = Assembly.GetEntryAssembly()!.FullName!.Split('.').Skip(1)
                .First().ToLower();
        }
    }

    private void CoalesceQueue()
    {
        if (string.IsNullOrWhiteSpace(Queue))
        {
            Queue = $"queue-{LowerName}-{Consumer}";
        }
    }
}
