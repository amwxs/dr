using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dr.Extensions.Logging.Abstractions;
internal class StructLogBuilder : IStructLogBuilder, IDisposable
{
    private readonly IEnhancerAccessor _enhancerAccessor;
    private readonly IHostInformation _hostInformation;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDisposable? _onChangeToken;
    private LoggerOptions _options;

    public StructLogBuilder(IEnhancerAccessor enhancerAccessor, 
        IHostInformation hostInformation, 
        IOptionsMonitor<LoggerOptions> options, 
        IDateTimeProvider dateTimeProvider)
    {
        _enhancerAccessor = enhancerAccessor;
        _hostInformation = hostInformation;
        _options = options.CurrentValue;
        _onChangeToken = options.OnChange(c => _options = c);
        _dateTimeProvider = dateTimeProvider;
    }

    public StructLog Build<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (state is not StructLog log)
        {
            log = new StructLog
            {
                Message = state?.ToString(),
                Exception = exception?.ToString()
            };
        }
        var enhancer = CurrentLogEnhancer();
        log.LogLevel = (int)logLevel;
        log.EventId = eventId.Id;

        log.AppId = _options.AppId;
        log.TraceId = enhancer?.TraceId;
        log.SpanId = enhancer?.SpanId;
        log.ParentSpanId = enhancer?.ParentSpanId;
        log.HostIp = _hostInformation.HostIp();
        log.CreateTime = _dateTimeProvider.Current;
        return log;
    }



    private LogEnhancer? CurrentLogEnhancer()
    {
        var enhancer = _enhancerAccessor.Current;
        if (enhancer == null)
        {
            return default;
        }

        if (!enhancer.Items.TryGetValue(EnhancerConst.EnhancerTrace, out var logEnhancer))
        {
            return default;
        };
        return logEnhancer as LogEnhancer;
    }

    public void Dispose()
    {
        _onChangeToken?.Dispose();
    }
}
