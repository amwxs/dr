using Dr.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Dr.Logging.RabbitMQ;
public class MQClient : IMQClient, IDisposable
{

    private RabbitMQOptions _options;
    private readonly IDisposable? _onChangeToken;
    private readonly ILocalFileWriter _localFileWriter;

    private IConnection? _connection;
    private IModel? _channel;
    private IBasicPublishBatch? _basicPublish;

    public MQClient(IOptionsMonitor<RabbitMQOptions> options, ILocalFileWriter localFileWriter)
    {
        _options = options.CurrentValue;
        _onChangeToken = options.OnChange(c => { _options = c; });
        _localFileWriter = localFileWriter;
        Connect();
    }

    public void BasicPublish(List<StructLog> structLogs)
    {
        if (_basicPublish == null)
        {
            if (!ReSetPublish())
            {
                //Note! MQ is disabel logs to local file
                _localFileWriter.Log(new LocalFileMessage { FileName = "log", Message = structLogs.ToJson() });
                return;
            }
        }
        foreach (var log in structLogs)
        {
            var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(log.ToJson()));
            _basicPublish.Add(_options.Exchange, string.Empty, false, null, body);
        }
        _basicPublish!.Publish();
    }

    private bool ReSetPublish()
    {
        if (_connection!.IsOpen)
        {
            if (_channel!.IsOpen)
            {
                _basicPublish = _channel.CreateBasicPublishBatch();
            }
            else
            {
                _channel?.Dispose();
                _channel = _connection.CreateModel();
                _basicPublish = _channel.CreateBasicPublishBatch();
            }
        }
        return _basicPublish != null;
    }

    private void Connect()
    {
        try
        {

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                VirtualHost = _options.VirtualHost,
                UserName = _options.UserName,
                Password = _options.Password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += Connection_ConnectionShutdown;

            _channel = _connection.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);
            _basicPublish = _channel.CreateBasicPublishBatch();
        }
        catch (Exception ex)
        {
            _localFileWriter.Log(new LocalFileMessage { Message = $"Reconnect Error: {ex.Message}" });
            throw;
        }

    }

    private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _basicPublish = null;
        _localFileWriter.Log(new LocalFileMessage { Message = e.ReplyText });

    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        _onChangeToken?.Dispose();
    }
}
