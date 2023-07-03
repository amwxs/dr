using Microsoft.Extensions.Logging;

namespace Dr.Logging.Abstractions;
internal class StructLogBuilder : IStructLogBuilder
{
    private readonly IEnhancerAccessor _enhancerAccessor;

    public StructLogBuilder(IEnhancerAccessor enhancerAccessor)
    {
        _enhancerAccessor = enhancerAccessor;
    }

    public StructLog Build<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

        var enhancer = CurrentLogEnhancer();
        var log = new StructLog
        {
            AppId = enhancer?.AppId,
            TraceId = enhancer?.TraceId,
            SpanId = enhancer?.SpanId,
            ParentSpanId = enhancer?.ParentSpanId
        };
        return log;
    }

    private  LogEnhancer? CurrentLogEnhancer()
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
