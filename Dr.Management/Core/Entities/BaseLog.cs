namespace Dr.Management.Core.Entities;

public class BaseLog
{
    //public string? Id { get; set; }
    public string HostIp { get; set; }
    public string AppId { get; set; }
    public string TraceId { get; set; }
    public string SpanId { get; set; }
    public string ParentSpanId { get; set; }
    public int LogLevel { get; set; }
    public int EventId { get; set; }
    public long Elapsed { get; set; }
    public DateTime CreateTime { get; set; }
}
