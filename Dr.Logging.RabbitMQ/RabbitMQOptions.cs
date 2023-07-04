namespace Dr.Logging.RabbitMQ;
public class RabbitMQOptions
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";

    public string Exchange { get; set; } = "drlogs";
} 

//(string HostName, int Port, string VirtualHost, string Exchange, string UserName,
//                              string Password);
