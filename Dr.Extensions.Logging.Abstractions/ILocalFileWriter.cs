namespace Dr.Extensions.Logging.Abstractions;

public interface ILocalFileWriter
{
    Task Log(LocalFileMessage localLog);
}