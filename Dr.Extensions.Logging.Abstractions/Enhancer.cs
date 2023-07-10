using System.Collections.Concurrent;

namespace Dr.Extensions.Logging.Abstractions;

public class Enhancer : IDisposable
{

    private readonly ConcurrentDictionary<string, object> _data = new ();
    private readonly Action _cleanAction;

    public Enhancer(Action cleanAction)
    {
        _cleanAction = cleanAction;
    }

    public bool TryAdd(string key, object value)
    {
        return _data.TryAdd(key, value);
    }


    public ConcurrentDictionary<string, object> Items => _data;


    public void Dispose()
    {
        _cleanAction();
    }
}
