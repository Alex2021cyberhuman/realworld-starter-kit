using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Conduit.Shared.Events.Services.RabbitMQ;

public abstract class BaseRabbitMqEventConsumer<T> : IHostedService, IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly RabbitMqSettings<T> _settings;
    private IModel? _channel;
    private IConnection? _connection;
    private InternalConsumer? _consumer;

    protected BaseRabbitMqEventConsumer(
        ConnectionFactory connectionFactory,
        ILoggerFactory loggerFactory,
        IServiceScopeFactory serviceScopeFactory,
        RabbitMqSettings<T> settings)
    {
        _connectionFactory = connectionFactory;
        _loggerFactory = loggerFactory;
        _serviceScopeFactory = serviceScopeFactory;
        _settings = settings;
    }

    protected IConnection Connection =>
        _connection ??= _connectionFactory.CreateConnection();

    protected IModel Channel => _channel ??= Connection.CreateModel();

    protected virtual InternalConsumer Consumer => _consumer ??=
        new(Channel, _serviceScopeFactory, _loggerFactory);

    public void Dispose()
    {
        Unbind();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(
        CancellationToken cancellationToken)
    {
        _settings.Initialize(Channel);
        Channel.BasicConsume(Consumer, _settings.Queue);
        return Task.CompletedTask;
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
    {
        Unbind();
        return Task.CompletedTask;
    }

    private void Unbind()
    {
        Channel.Close();
        Connection.Close();
        Channel.Dispose();
        Connection.Dispose();
        _channel = null;
        _connection = null;
    }

    public class InternalConsumer : AsyncDefaultBasicConsumer
    {
        private readonly IModel _channel;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public InternalConsumer(
            IModel channel,
            IServiceScopeFactory scopeFactory,
            ILoggerFactory loggerFactory) : base(channel)
        {
            _channel = channel;
            _scopeFactory = scopeFactory;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public override async Task HandleBasicDeliver(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IBasicProperties properties,
            ReadOnlyMemory<byte> body)
        {
            var message = body.DeBytorizeAsRequiredJson<T>();
            await using var scope = _scopeFactory.CreateAsyncScope();
            var eventConsumer = scope.ServiceProvider
                .GetRequiredService<IEventConsumer<T>>();
            try
            {
                await eventConsumer.ConsumeAsync(message);
                _channel.BasicAck(deliveryTag, false);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Exception during message consumption");
            }
        }
    }
}
