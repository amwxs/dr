namespace Dr.Management.Core.Entities;

public class EntireLog: BaseLog
{
    public string? Message { get; set; }
    public string? Exception { get; set; }

    //aspnet core
    public Request Request { get; set; } = new();

    public Response Response { get; set; } = new();

}

public class Request
{
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? Body { get; set; }
    public IEnumerable<string>? Headers { get; set; }
}

public class Response
{
    public int StatusCode { get; set; }
    public string? Body { get; set; }
    public IEnumerable<string>? Headers { get; set; }
}