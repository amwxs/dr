namespace Dr.Management.Core.Entities;

public class TreeLog: BaseLog
{
    public List<TreeLog>? SubLogs { get; set; }
}
