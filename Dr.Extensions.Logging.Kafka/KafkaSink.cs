using Dr.Extensions.Logging.Abstractions;

namespace Dr.Extensions.Logging.Kafka;
internal class KafkaSink : ILogSink
{
    private readonly IKafkaClient _kafkaClient;

    public KafkaSink(IKafkaClient kafkaClient)
    {
        _kafkaClient = kafkaClient;
    }

    public void Write(List<StructLog> structLogs)
    {
        _kafkaClient.BasicPublish(structLogs);
    }
}
