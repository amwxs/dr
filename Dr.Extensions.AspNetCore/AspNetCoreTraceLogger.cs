using Dr.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace Dr.Logging.AspNetCore;
public static class AspNetCoreTraceLogger
{

    public static void HttpTrace(this ILogger logger,StructLog structLog)
    {
        logger.Log(LogLevel.Information, new EventId(-100), structLog, null, (x, s) => string.Empty);
    }
}
