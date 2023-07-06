namespace Dr.Loggin.Collector;
public interface ICollector
{
    public event EventHandler<StructLogEventArgs> OnReceived;
}
