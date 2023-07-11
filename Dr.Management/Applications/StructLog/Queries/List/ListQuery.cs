using Dr.Management.Core;
using Dr.Management.Core.Entities;
using MediatR;

namespace Dr.Management.Applications.StructLog.Queries.List;

public class ListQuery : IRequest<CustResult<List<BaseLog>>>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    // base contions
    public string? HostIp { get; set; }
    public string? AppId { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public string? ParentSpanId { get; set; }
    public int LogLevel { get; set; } = -999;
    public int EventId { get; set; } = -999;
    public string? Message { get; set; }
    public string? Exception { get; set; }
    public DateTime? StartCreateTime { get; set; }
    public DateTime? EndCreateTime { get; set; }


    // request conditions
    public string? RequestPath { get; set; }
    public string? RequestMethod { get; set; }
    public string? RequestBody { get; set; }
    public string? RequestHeader { get; set; }

    // Response Conditions
    public int ResponseStatusCode { get; set; } = -999;
    public string? ResponseBody { get; set; }
    public string? ResponseHeader { get; set; }


}
