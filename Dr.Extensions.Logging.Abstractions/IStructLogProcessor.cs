namespace Dr.Extensions.Logging.Abstractions;

public interface IStructLogProcessor: IDisposable
{
    void AddLog(StructLog structLog);

    void ConsolePrint(StructLog structLog);

    void ConsumerLog();
}