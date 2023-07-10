using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.Abstractions.Test;
public class LoggerProviderTest
{

    [Fact]
    public void CreateLoggerTest()
    {
        var factory = LoggerFactory.Create(c => c.AddDrLogger(o =>
        {
            o.IsConsolePrint = true;
            o.LogLevel.Add("Default", LogLevel.Information);
        }));
        var log = factory.CreateLogger("LoggerProviderTest");
        log.LogInformation("hello");
    }
}
