namespace Dr.Management.Core;

public class Pager
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public long Total { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex * PageSize < Total;

    public Pager(int pageNo, int pageSize, long total)
    {
        PageIndex = pageNo;
        PageSize = pageSize;
        Total = total;
    }
}
