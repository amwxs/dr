using Microsoft.Extensions.Logging;

namespace Dr.Logging.Abstractions;
public interface ILogLevelFilter
{
    bool IsEnabled(string categoryName, LogLevel logLevel);
}