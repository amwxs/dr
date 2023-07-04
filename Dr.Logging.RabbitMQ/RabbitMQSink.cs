using Dr.Logging.Abstractions;

namespace Dr.Logging.RabbitMQ;
public class RabbitMQLogSink : ILogSink
{
    void ILogSink.Write(List<StructLog> structLogs)
    {
        foreach (var log in structLogs)
        {
            Console.WriteLine(log.ToJson());
        }
    }
}
