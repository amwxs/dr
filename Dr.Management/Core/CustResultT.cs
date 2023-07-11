namespace Dr.Management.Core;
public class CustResult<T> : CustResult
{
    public T? Data { get; set; }
    public Pager? Pager { get; set; }
}