namespace Dr.Logging.Abstractions;

public interface ILocalFileWriter
{
    void Write(LocalFileMessage localLog);
}