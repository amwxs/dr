namespace Dr.Extensions.Logging.Abstractions;

public class LogEnhancer
{
    public string? AppId { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public string? ParentSpanId { get; set; }
}
