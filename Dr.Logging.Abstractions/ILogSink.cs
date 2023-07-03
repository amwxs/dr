namespace Dr.Logging.Abstractions;
public interface ILogSink
{
    void Write(List<StructLog> structLog);
}
