using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.Abstractions;
public interface ILogLevelFilter
{
    bool IsEnabled(string categoryName, LogLevel logLevel);
}