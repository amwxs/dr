namespace Dr.Management.Core.Entities;

public class TraceLog: BaseLog
{
    public List<TraceLog>? Subs { get; set; }
}
