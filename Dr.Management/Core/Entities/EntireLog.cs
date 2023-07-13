using Nest;

namespace Dr.Management.Core.Entities;

public class EntireLog: BaseLog
{

    [Text(Name = "Message")]
    public string? Message { get; set; }
    [Text(Name = "Exception")]
    public string? Exception { get; set; }

    [Nested(Name= "Request")]
    public Request Request { get; set; } = new();

    [Nested(Name = "Response")]
    public Response Response { get; set; } = new();

}

public class Request
{
    [Text(Name = "Path")]
    public string? Path { get; set; }
    [Keyword(Name = "Method")]
    public string? Method { get; set; }
    [Text(Name = "Body")]
    public string? Body { get; set; }
    [Nested(Name = "Headers")]
    public IEnumerable<string>? Headers { get; set; }
}

public class Response
{
    [Number(Name = "StatusCode")]
    public int StatusCode { get; set; }
    [Text(Name = "Body")]
    public string? Body { get; set; }
    [Nested(Name = "Headers")]
    public IEnumerable<string>? Headers { get; set; }
}