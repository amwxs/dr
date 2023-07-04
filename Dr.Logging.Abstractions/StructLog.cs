namespace Dr.Logging.Abstractions;
public class StructLog
{
    public string? HostIp { get;  set; }
    public string? AppId { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public string? ParentSpanId { get; set; }
    public int LogLevel { get; set; }
    public int EventId { get; set; }
    public string? Message { get; set; }
    public string? Exception { get;  set; }
    public DateTime CreateTime { get;  set; }
}
