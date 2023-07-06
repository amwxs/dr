using Dr.Logging.Abstractions;

namespace Dr.Loggin.Collector.RabbitMQ;
public class RabbitMQCollector : ICollector
{
    public event EventHandler<StructLogEventArgs>? OnReceived;

    public void Received(StructLog log)
    {
        OnReceived?.Invoke(this, new StructLogEventArgs(log));
    }
}
