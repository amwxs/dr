namespace Dr.Management.Core;

public class Pager
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex * PageSize < TotalCount;

    public Pager(int pageNo, int pageSize, long totalCount)
    {
        PageIndex = pageNo;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
