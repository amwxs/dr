using Nest;

namespace Dr.Management.Core.Entities;


public class BaseLog
{
    [Ignore]
    public string? Id { get; set; }

    [Keyword(Name = "HostIp")]
    public string? HostIp { get; set; }

    [Keyword(Name = "AppId")]
    public string? AppId { get; set; }

    [Keyword(Name = "TraceId")]
    public string? TraceId { get; set; }

    [Keyword(Name = "SpanId")]
    public string? SpanId { get; set; }

    [Keyword(Name = "ParentSpanId")]
    public string? ParentSpanId { get; set; }

    [Number(Name= "LogLevel")]
    public int LogLevel { get; set; }

    [Number(Name = "EventId")]
    public int EventId { get; set; }

    [Number(Name = "Elapsed")]
    public long Elapsed { get; set; }

    [Text(Name = "Message")]
    public string? Message { get; set; }

    [Text(Name = "Exception")]
    public string? Exception { get; set; }

    [Date(Name = "CreateTime")]
    public DateTime CreateTime { get; set; }
}
