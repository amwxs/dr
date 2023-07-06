namespace Dr.Logging.Abstractions;

public interface ILocalFileWriter
{
    Task Log(LocalFileMessage localLog);
}