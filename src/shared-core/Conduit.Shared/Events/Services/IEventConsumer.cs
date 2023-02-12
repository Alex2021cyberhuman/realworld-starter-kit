namespace Conduit.Shared.Events.Services;

public interface IEventConsumer<in T>
{
    Task ConsumeAsync(
        T message);
}
