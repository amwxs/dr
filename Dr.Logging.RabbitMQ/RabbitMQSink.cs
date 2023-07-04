using Dr.Logging.Abstractions;

namespace Dr.Logging.RabbitMQ;
public class RabbitMQSink : ILogSink
{
    private readonly IMQClient _mQClient;

    public RabbitMQSink(IMQClient mQClient)
    {
        _mQClient = mQClient;
    }

    void ILogSink.Write(List<StructLog> structLogs)
    {
        _mQClient.BasicPublish(structLogs);
    }
}
