using Microsoft.Extensions.Options;

namespace Dr.Logging.Abstractions;
public class LocalFileWriter : ILocalFileWriter, IDisposable
{
    private LoggerOptions _loggerOptions;
    private readonly IDisposable? _onChangeToken;

    public LocalFileWriter(IOptionsMonitor<LoggerOptions> options)
    {
        _loggerOptions = options.CurrentValue;
        _onChangeToken = options.OnChange(c => _loggerOptions = c);
    }

    private const string _suffix = ".log";
    public async  Task Log(LocalFileMessage localLog)
    {
        try
        {
            if (!Directory.Exists(_loggerOptions.LocalPath))
            {
                Directory.CreateDirectory(_loggerOptions.LocalPath);
            }
            var fullFilePath = Path.Combine(_loggerOptions.LocalPath, localLog.FileName + localLog.CreateTime.ToString("yyyy-MM-dd") + _suffix);

            using var writer = new StreamWriter(fullFilePath, true);
            await writer.WriteAsync($"datetime: {localLog.CreateTime} Message: {localLog.Message}");
        }
        catch (Exception)
        {
            throw;
        }
    }



    public void Dispose()
    {
        _onChangeToken?.Dispose();
    }
}
