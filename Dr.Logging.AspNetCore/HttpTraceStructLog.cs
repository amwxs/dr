using Dr.Logging.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Dr.Logging.AspNetCore;
public class HttpTraceStructLog: StructLog
{
    public Request Request { get; set; } = new();

    public Response Response { get; set; } = new();

    public long ElapsedMillisecond { get; set; }
}
public class Request
{
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? Body { get; set; }
    public IHeaderDictionary? Headers { get; set; }
}

public class Response
{
    public int StatusCode { get; set; }
    public string? Body { get; set; }
    public IHeaderDictionary? Headers { get; set; }
}

