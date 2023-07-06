using Dr.Logging.Abstractions;

namespace Dr.Loggin.Collector;
public class StructLogEventArgs
{
    public StructLog Log { get;}
    public StructLogEventArgs(StructLog structLog)
    {
        Log = structLog;
    }
}
