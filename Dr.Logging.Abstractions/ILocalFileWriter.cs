namespace Dr.Logging.Abstractions;

public interface ILocalFileWriter
{
    void Log(LocalFileMessage localLog);
}