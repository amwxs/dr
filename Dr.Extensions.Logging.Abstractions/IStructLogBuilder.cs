using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.Abstractions;
public interface IStructLogBuilder: IDisposable
{
    StructLog Build<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);
}