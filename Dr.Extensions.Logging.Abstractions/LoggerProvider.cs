using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Dr.Extensions.Logging.Abstractions;
public class LoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, Logger> _loggers = new();
    private readonly ILogLevelFilter _logLevelFilter;
    private readonly IStructLogProcessor _structLogProcessor;
    private readonly IStructLogBuilder _structLogBuilder;
    public LoggerProvider(
        ILogLevelFilter logLevelFilter,
        IStructLogProcessor structLogProcessor,
        IStructLogBuilder structLogBuilder)
    {
        _structLogProcessor = structLogProcessor;
        _logLevelFilter = logLevelFilter;
        _structLogBuilder = structLogBuilder;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name 
            => new Logger(categoryName, _logLevelFilter, _structLogBuilder, _structLogProcessor));
    }


    public void Dispose()
    {
        _structLogProcessor?.Dispose();
        _structLogBuilder?.Dispose();
        _logLevelFilter?.Dispose();
    }
}
