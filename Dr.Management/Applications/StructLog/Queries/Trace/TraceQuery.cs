using Dr.Management.Core.Entities;
using Dr.Management.Core;
using MediatR;

namespace Dr.Management.Applications.StructLog.Queries.Trace;

public class TraceQuery : IRequest<CustResult<List<TreeLog>>>
{
    public string? TraceId { get; set; }
}
