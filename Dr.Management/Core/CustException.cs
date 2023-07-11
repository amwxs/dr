namespace Dr.Management.Core;

public class CustException : Exception
{
    public string Code { get; }

    public CustException(string code, string message)
        : base(message)
    {
        Code = code;
    }
    public CustException(string code, string message, Exception innerException)
        : base(message, innerException)
    {
        Code = code;
    }
}
