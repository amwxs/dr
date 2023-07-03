namespace Dr.Logging.Abstractions;
public class StructLog
{
    public string? AppId { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public string? ParentSpanId { get; set; }
    public int LogLevel { get; set; }
    public string? Message { get; set; }
}
