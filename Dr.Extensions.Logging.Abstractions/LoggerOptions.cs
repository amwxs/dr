using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.Abstractions;
public class LoggerOptions
{
    public string? AppId { get; set; }
    public Dictionary<string, LogLevel> LogLevel { get; set; } = new();
    public int QueuecCapacity { get;  set; } = 10000;
    public int BatchSize { get; set; } = 1000;
    public int RefreshInterval { get; set; } = 5;
    public string LocalPath { get; set; } = "/var/logs/";
    public bool IsConsolePrint { get; set; }
}


