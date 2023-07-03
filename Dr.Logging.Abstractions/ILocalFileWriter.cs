namespace Dr.Logging.Abstractions;

internal interface ILocalFileWriter
{
    void Write(LocalFileMessage localLog);
}