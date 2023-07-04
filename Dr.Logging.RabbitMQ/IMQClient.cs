using Dr.Logging.Abstractions;

namespace Dr.Logging.RabbitMQ;
public interface IMQClient
{
    void BasicPublish(List<StructLog> structLogs);
}