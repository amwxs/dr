using Dr.Loggin.Collector;
using Dr.Logging.Abstractions;
using System.Collections.Concurrent;

namespace Dr.Logging.Server.Core;

public class CollectorProcessor : ICollectorProcessor
{
    private readonly ICollector _collector;
    private readonly BlockingCollection<StructLog> _structLogQueue;
    public CollectorProcessor(ICollector collector)
    {
        _collector = collector;
        _structLogQueue = new BlockingCollection<StructLog>(100000);
    }

    public void Start()
    {
        _collector.OnReceived += Collector_OnReceived;
    }

    private void Collector_OnReceived(object? sender, StructLogEventArgs e)
    {

        if (!_structLogQueue.TryAdd(e.Log))
        {
            while (!_structLogQueue.TryAdd(e.Log,500))
            {
                Thread.Sleep(1000);
            }

        }
    }

    public void Stop()
    {

    }
}
