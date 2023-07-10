using Dr.Extensions.Logging.Abstractions;

namespace Dr.Extensions.Logging.RabbitMQ;
public interface IMQClient
{
    void BasicPublish(List<StructLog> structLogs);
}