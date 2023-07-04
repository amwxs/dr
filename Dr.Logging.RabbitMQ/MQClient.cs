using Dr.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Dr.Logging.RabbitMQ;
public class MQClient : IMQClient, IDisposable
{

    private RabbitMQOptions _options;
    private readonly IDisposable? _onChangeToken;
    private readonly ILocalFileWriter _localFileWriter;

    private IConnection _connection;
    private IModel _channel;

    public MQClient(IOptionsMonitor<RabbitMQOptions> options, ILocalFileWriter localFileWriter)
    {
        _options = options.CurrentValue;
        _onChangeToken = options.OnChange(c => { _options = c; });
        _localFileWriter = localFileWriter;

        _connection = GetConnection();
        _connection.ConnectionShutdown += Connection_ConnectionShutdown;
        _channel = _connection.CreateModel();

    }


    private IConnection GetConnection()
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
            return factory.CreateConnection();
        }
        catch (Exception ex)
        {
            _localFileWriter.Log(new LocalFileMessage { Message = $"Connection RabbitMQ Error: {ex.Message}" });
            throw;
        }

    }



    public void BasicPublish(List<StructLog> structLogs)
    {
        if (_channel.IsClosed)
        {
            Reconnect();
        }

        foreach (var log in structLogs)
        {
            var body = Encoding.UTF8.GetBytes(log.ToJson());

            _channel.BasicPublish(_options.Exchange, string.Empty, _channel.CreateBasicProperties(), body);
        }
    }

    private void Reconnect()
    {
        try
        {
            if (_connection.IsOpen)
            {
                _channel = _connection.CreateModel();
            }
            else
            {
                _connection = GetConnection();
                _channel = _connection.CreateModel();
            }
        }
        catch (Exception ex)
        {
            _localFileWriter.Log(new LocalFileMessage { Message = $"Reconnect Error: {ex.Message}" });
        }

    }

    private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _localFileWriter.Log(new LocalFileMessage { Message = e.ReplyText });
    }

    public void Dispose()
    {
        _onChangeToken?.Dispose();
    }
}
