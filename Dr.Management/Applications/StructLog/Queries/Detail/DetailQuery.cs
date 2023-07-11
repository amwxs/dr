using Dr.Management.Core;
using Dr.Management.Core.Entities;
using MediatR;

namespace Dr.Management.Applications.StructLog.Queries.Detail;

public class DetailQuery : IRequest<CustResult<EntireLog>>
{
    public string? Id { get; set; }
}
