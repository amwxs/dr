using Dr.Extensions.Logging.Abstractions;

namespace Dr.Extensions.Logging.Kafka;
public interface IKafkaClient
{
    void BasicPublish(List<StructLog> structLogs);
}
