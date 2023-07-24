namespace Dr.Extensions.Logging.Abstractions;
public interface ISink
{
    void Write(List<StructLog> structLogs);
}
