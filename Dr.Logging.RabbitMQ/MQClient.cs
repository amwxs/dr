using Dr.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Dr.Logging.RabbitMQ;
public class MQClient : IMQClient
{

    private readonly RabbitMQOptions _options;
    private IConnection _connection;
    private IModel _channel;

    public MQClient(IOptions<RabbitMQOptions> options)
    {
        _options = options.Value;
        _connection = GetConnection();
        _connection.ConnectionShutdown += Connection_ConnectionShutdown;
        _channel = _connection.CreateModel();
    }


    private IConnection GetConnection()
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

    private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {

    }
}
