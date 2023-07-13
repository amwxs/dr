using Microsoft.Extensions.Options;

namespace Dr.Extensions.Logging.Abstractions;
public class LocalFileWriter : ILocalFileWriter, IDisposable
{
    private LoggerOptions _loggerOptions;
    private readonly IDisposable? _onChangeToken;
    private readonly IDateTimeProvider _dateTimeProvider;
    public LocalFileWriter(IOptionsMonitor<LoggerOptions> options, IDateTimeProvider dateTimeProvider)
    {
        _loggerOptions = options.CurrentValue;

        CreateDirectory();
        _onChangeToken = options.OnChange(c =>
        {
            _loggerOptions = c;
            CreateDirectory();
        });
        _dateTimeProvider = dateTimeProvider;
    }

    private void CreateDirectory()
    {
        if (!Directory.Exists(_loggerOptions.LocalPath))
        {
            Directory.CreateDirectory(_loggerOptions.LocalPath);
        }
    }

    private const string _suffix = ".log";
    public async  Task Log(LocalFileMessage localLog)
    {
        try
        {
            
            var fullFilePath = Path.Combine(_loggerOptions.LocalPath, localLog.FileName + _dateTimeProvider.Current.ToString("yyyy-MM-dd") + _suffix);

            using var writer = new StreamWriter(fullFilePath, true);
            await writer.WriteAsync($"datetime: {_dateTimeProvider.Current} Message: {localLog.Message}");
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
