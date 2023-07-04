

using Microsoft.Extensions.Logging;
using Dr.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Dr.Logging.RabbitMQ;

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.ClearProviders()
        .AddDrLogger(options =>
        {
            options.IsConsolePrint = true;
            options.LogLevel.Add("Default", LogLevel.Information);
        })
        .RabbitMQSink();
    }).BuildServiceProvider();

var loggerFactor = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactor.CreateLogger("LoggerProviderTest");
var ehancerAccessor = serviceProvider.GetRequiredService<IEnhancerAccessor>();

using (var enchaner = ehancerAccessor.Create())
{
    enchaner.TryAdd("logEnhancer", new LogEnhancer 
    {
        AppId= Guid.NewGuid().ToString("N"),
        TraceId = Guid.NewGuid().ToString("N"),
        SpanId = Guid.NewGuid().ToString("N"),
        ParentSpanId = Guid.NewGuid().ToString("N")
    });

   

    //logger.Log(LogLevel.Information, new EventId(100), new StructLog 
    //{
    //   Message = "Hello StructLog"
    //}, null, (l, e) => default!);

    for (int i = 0; i < 50; i++)
    {
        logger.LogInformation("info hello{i}", i);
        logger.LogWarning("warn hello{i}", i);
        logger.LogError("error hello{i}", i);
    }
}
//logger.LogInformation("Hello No AppId Infromation  测试下中文");


Console.ReadKey();