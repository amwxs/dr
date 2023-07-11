using Dr.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Dr.Extensions.Logging.RabbitMQ;
public class MQClient : IMQClient, IDisposable
{

    private RabbitMQOptions _options;
    private readonly IDisposable? _onChangeToken;
    private readonly ILocalFileWriter _localFileWriter;

    private readonly IConnection _connection;
    private IModel _channel;
    private bool _isConnect = true;

    public MQClient(IOptionsMonitor<RabbitMQOptions> options, ILocalFileWriter localFileWriter)
    {
        _options = options.CurrentValue;
        _onChangeToken = options.OnChange(c => { _options = c; });
        _localFileWriter = localFileWriter;
        (_connection, _channel) = Connect();
    }

    public void BasicPublish(List<StructLog> structLogs)
    {
        if (!_isConnect)
        {
            if (!ReConnect())
            {
                //Note! MQ is disabel logs to local file
                _localFileWriter.Log(new LocalFileMessage { FileName = "log", Message = structLogs.ToJson() });
                return;
            }
        }
        var batch = _channel.CreateBasicPublishBatch();
        foreach (var log in structLogs)
        {
            var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(log.ToJson()));
            batch.Add(_options.Exchange, string.Empty, false, null, body);
        }
        batch.Publish();
    }

    private bool ReConnect()
    {
        if (!_connection.IsOpen)
        {
            return false;
        }
        if (!_channel.IsOpen)
        {
            _channel?.Dispose();
            _channel = _connection.CreateModel();
        }
        _isConnect = true;
        return true;
    }

    private (IConnection connection, IModel channel) Connect()
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

            var connection = factory.CreateConnection();
            connection.ConnectionShutdown += Connection_ConnectionShutdown;

            var channel = connection.CreateModel();
            return (connection, channel);
        }
        catch (Exception ex)
        {
            _localFileWriter.Log(new LocalFileMessage { Message = $"Reconnect Error: {ex.Message}" });
            throw;
        }

    }

    private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _isConnect = false;
        _localFileWriter.Log(new LocalFileMessage { Message = e.ReplyText });

    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        _onChangeToken?.Dispose();
    }
}
