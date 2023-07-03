using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Dr.Logging.Abstractions;
internal class StructLogBuilder : IStructLogBuilder
{
    private readonly IEnhancerAccessor _enhancerAccessor;
    private string _hostIp = string.Empty;

    public StructLogBuilder(IEnhancerAccessor enhancerAccessor)
    {
        _enhancerAccessor = enhancerAccessor;
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
        log.HostIp = GetHostIp();
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

    private string GetHostIp()
    {
        if (!string.IsNullOrEmpty(_hostIp))
        {
            return _hostIp;
        }
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        var ipAddresses = networkInterfaces
            .Where(ni => ni.NetworkInterfaceType != NetworkInterfaceType.Loopback && !ni.Description.ToLower().Contains("virtual"))
            .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
            .Where(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork)
            .Select(ua => ua.Address.ToString())
            .ToList();
        if (ipAddresses.Count > 0)
        {
            _hostIp = ipAddresses[0];

        }
        return _hostIp;
    }
}
