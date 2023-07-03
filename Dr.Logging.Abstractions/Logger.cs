using Microsoft.Extensions.Logging;

namespace Dr.Logging.Abstractions;
internal class Logger : ILogger
{
    private readonly string _categoryName;
    private readonly ILogLevelFilter _logLevelFilter;
    private readonly IStructLogBuilder _structLogBuilder;
    private readonly IStructLogProcessor _structLogProcessor;
    public Logger(string categoryName,
        ILogLevelFilter logLevelFilter,
        IStructLogBuilder structLogBuilder,
        IStructLogProcessor structLogProcessor)
    {
        _categoryName = categoryName;
        _logLevelFilter = logLevelFilter;
        _structLogProcessor = structLogProcessor;
        _structLogBuilder = structLogBuilder;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logLevelFilter.IsEnabled(_categoryName, logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        var log = _structLogBuilder.Build(logLevel, eventId, state, exception, formatter);
        _structLogProcessor.AddLog(log);

        //NOTE If the log console print is enabled, it will be printed to the console.
        _structLogProcessor.ConsolePrint(log);
    }

}
