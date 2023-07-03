using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Dr.Logging.Abstractions;
internal class LogLevelFilter : ILogLevelFilter, IDisposable
{
    private readonly ConcurrentDictionary<string, LogLevel> _loggerLogLevels = new();
    private readonly IDisposable? _onChangeToken;
    private LoggerOptions _loggerOptions;
    public LogLevelFilter(IOptionsMonitor<LoggerOptions> options)
    {
        _loggerOptions = options.CurrentValue;
        _onChangeToken = options.OnChange(c => { _loggerOptions = c; });
    }

    public bool IsEnabled(string categoryName, LogLevel logLevel)
    {
        foreach (var item in GetKeyPrefix(categoryName))
        {
            if (_loggerOptions.LogLevel.TryGetValue(item, out var level))
            {
                return level <= logLevel;
            }
        }
        return false;
    }

    private static IEnumerable<string> GetKeyPrefix(string name)
    {
        while (!string.IsNullOrEmpty(name))
        {
            yield return name;
            var lastIndexOfDot = name.LastIndexOf('.');
            if (lastIndexOfDot == -1)
            {
                yield return "Default";
                break;
            }
            name = name[..lastIndexOfDot];
        }
    }

    public void Dispose()
    {
        _onChangeToken?.Dispose();
    }
}
