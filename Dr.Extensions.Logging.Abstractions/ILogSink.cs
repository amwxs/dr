namespace Dr.Extensions.Logging.Abstractions;
public interface ILogSink
{
    void Write(List<StructLog> structLogs);
}
