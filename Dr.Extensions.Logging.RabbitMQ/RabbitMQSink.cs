using Dr.Extensions.Logging.Abstractions;

namespace Dr.Extensions.Logging.RabbitMQ;
public class RabbitMQSink : ISink
{
    private readonly IMQClient _mQClient;

    public RabbitMQSink(IMQClient mQClient)
    {
        _mQClient = mQClient;
    }

    void ISink.Write(List<StructLog> structLogs)
    {
        _mQClient.BasicPublish(structLogs);
    }
}
