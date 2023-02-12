namespace Conduit.Shared.Events.Services;

public interface IEventProducer<in T>
{
    Task ProduceEventAsync(
        T message);
}
