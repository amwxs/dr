using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Dr.Logging.Abstractions;
internal class StructLogProcessor : IStructLogProcessor, IDisposable
{
    private LoggerOptions _loggerOptions;
    private readonly IConsoleColorPrint _consoleColorPrint;
    private readonly ILocalFileWriter _localFileWriter;
    private readonly ILogSink _logSink;
    private readonly BlockingCollection<StructLog> _structLogQueue;
    private readonly IDisposable? _onChangeToken;
    private readonly Thread _outputThread;

    public StructLogProcessor(
        IOptionsMonitor<LoggerOptions> options, 
        IConsoleColorPrint consoleColorPrint, 
        ILocalFileWriter localFileWriter,
        ILogSink logSink)
    {
        _loggerOptions = options.CurrentValue;
        _onChangeToken = options.OnChange(c => { _loggerOptions = c; });

        _consoleColorPrint = consoleColorPrint;
        _localFileWriter = localFileWriter;
        _logSink = logSink;

        _structLogQueue = new BlockingCollection<StructLog>(_loggerOptions.QueuecCapacity);
        _outputThread = new Thread(ConsumerLog)
        {
            IsBackground = true,
            Name = "Console logger queue processing thread",
        };
        _outputThread.Start();

    }

    public void AddLog(StructLog structLog)
    {
        if (!_structLogQueue.TryAdd(structLog))
        {
            _localFileWriter.Log(new LocalFileMessage { Message = "Failed to enqueue the log." });
        }
    }

    public void ConsolePrint(StructLog structLog)
    {
        if (_loggerOptions.IsConsolePrint)
        {
            _consoleColorPrint.Print(structLog);
        }
    }

    public void ConsumerLog()
    {

        var batchLogs = new List<StructLog>();
        while (true)
        {
            try
            {
                var start = DateTime.UtcNow;

                while (batchLogs.Count < _loggerOptions.BatchSize
                    && (DateTime.UtcNow - start).TotalSeconds < _loggerOptions.RefreshInterval)
                {
                    if (_structLogQueue.TryTake(out var item, 500))
                    {
                        batchLogs.Add(item);
                    }
                    else
                    {
                        break;
                    }
                }

                if (batchLogs.Count > 0)
                {
                    _logSink.Write(batchLogs);

                    batchLogs.Clear();
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                _localFileWriter.Log(new LocalFileMessage { Message= ex.Message });
            }
        }
    }

    public void Dispose()
    {

        try
        {
            //阻塞主线程 刷新日志
            _outputThread.Join(1000);
        }
        catch (ThreadStateException) { }

        _onChangeToken?.Dispose();
    }
}
