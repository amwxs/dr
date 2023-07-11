namespace Dr.Management.Core;


public class CustResult
{
    private const string _defualtOk = "0";
    public string Code { get; set; } = _defualtOk;

    public string? Message { get; set; }

    public IEnumerable<Error>? Errors { get; set; }


    public static CustResult<T> Success<T>(T data, Pager? pager = null)
    {
        var response = new CustResult<T> { Data = data, Pager = pager, };
        return response;
    }
    public static CustResult<T> Failure<T>(string code, string message, IEnumerable<Error>? validationErrors=null)
    {
        var response = new CustResult<T> { Code = code, Message = message, Errors = validationErrors };
        return response;
    }
}
