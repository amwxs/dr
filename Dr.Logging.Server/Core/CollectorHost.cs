namespace Dr.Logging.Server.Core;

public class CollectorHost : IHostedService
{
    private readonly ICollectorProcessor _collector;

    public CollectorHost(ICollectorProcessor collector)
    {
        _collector = collector;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        //取消息队列数据
        _collector.Start();

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _collector.Stop();
        await Task.CompletedTask;
    }
}
