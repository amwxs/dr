using Dr.Management.Applications.StructLog.Queries.Detail;
using Dr.Management.Applications.StructLog.Queries.List;
using Dr.Management.Applications.StructLog.Queries.Trace;
using Dr.Management.Core;
using Dr.Management.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dr.Management.Controllers;
[ApiController]
[Route("[controller]")]
public class LoggingController : ControllerBase
{
    private readonly ISender _sender;

    public LoggingController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("query")]
    public async Task<CustResult<List<BaseLog>>> Query([FromQuery] ListQuery query)
    {
        return await _sender.Send(query);
    }

    [HttpPost("trace")]
    public async Task<CustResult<List<TreeLog>>> Trace([FromQuery] TraceQuery query)
    {
        return await _sender.Send(query);
    }

    [HttpGet("detail")]
    public async Task<CustResult<EntireLog>> Detail([FromQuery] DetailQuery query)
    {
        return await _sender.Send(query);
    }
}
