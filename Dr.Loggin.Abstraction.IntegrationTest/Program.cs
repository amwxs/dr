

using Microsoft.Extensions.Logging;
using Dr.Logging.Abstractions;

var factory = LoggerFactory.Create(c => c.AddDrLogger(o =>
{
    o.IsConsolePrint = true;
    o.LogLevel.Add("Default", LogLevel.Information);
}));

using (var enchaner = new EnhancerAccessor().Create())
{
    enchaner.TryAdd("logEnhancer", new LogEnhancer 
    {
        AppId= Guid.NewGuid().ToString("N"),
        TraceId = Guid.NewGuid().ToString("N"),
        SpanId = Guid.NewGuid().ToString("N"),
        ParentSpanId = Guid.NewGuid().ToString("N")
    });
    var log = factory.CreateLogger("LoggerProviderTest");
    log.LogInformation("hello");

    log.Log(LogLevel.Information, new EventId(100), new StructLog 
    {
       Message = "Hello StructLog"
    }, null, (l, e) => default!);
}


Console.ReadKey();