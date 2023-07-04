using Microsoft.Extensions.Logging;

namespace Dr.Logging.Abstractions;
internal class StructLogBuilder : IStructLogBuilder
{
    private readonly IEnhancerAccessor _enhancerAccessor;
    private readonly IHostInformation _hostInformation;

    public StructLogBuilder(IEnhancerAccessor enhancerAccessor, IHostInformation hostInformation)
    {
        _enhancerAccessor = enhancerAccessor;
        _hostInformation = hostInformation;
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

        log.AppId = enhancer?.AppId;
        log.TraceId = enhancer?.TraceId;
        log.SpanId = enhancer?.SpanId;
        log.ParentSpanId = enhancer?.ParentSpanId;
        log.HostIp = _hostInformation.HostIp();
        log.CreateTime = DateTime.UtcNow;
        return log;
    }


    public void AttachEnhancer(StructLog structLog)
    {
        var enhancer = CurrentLogEnhancer();
        structLog.AppId = enhancer?.AppId;
        structLog.TraceId = enhancer?.TraceId;
        structLog.SpanId = enhancer?.SpanId;
        structLog.ParentSpanId = enhancer?.ParentSpanId;
    }

    private LogEnhancer? CurrentLogEnhancer()
    {
        var enhancer = _enhancerAccessor.Current;
        if (enhancer == null)
        {
            return default;
        }

        if (!enhancer.Items.TryGetValue("logEnhancer", out var logEnhancer))
        {
            return default;
        };
        return logEnhancer as LogEnhancer;
    }
}
