

using Dr.Extensions.Logging.Abstractions;
using Dr.Extensions.Logging.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.ClearProviders()
        .AddDrLogger(options =>
        {
            //options.IsConsolePrint = true;
            options.AppId = "app.integration.test";
            options.LogLevel.Add("Default", LogLevel.Information);
        })
        .AddRabbitMQSink();
    }).BuildServiceProvider();

var loggerFactor = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactor.CreateLogger("LoggerProviderTest");
var ehancerAccessor = serviceProvider.GetRequiredService<IEnhancerAccessor>();

using (var enchaner = ehancerAccessor.Create())
{
    enchaner.TryAdd(EnhancerConst.EnhancerTrace, new LogEnhancer 
    {
        TraceId = Guid.NewGuid().ToString("N"),
        SpanId = Guid.NewGuid().ToString("N"),
        ParentSpanId = Guid.NewGuid().ToString("N")
    });

    //logger.Log(LogLevel.Information, new EventId(100), new StructLog
    //{
    //    Message = "Hello StructLog"
    //}, null, (l, e) => default!);
    for (int j = 0; j < 100; j++)
    {
        //for (int i = 0; i < 1000000; i++)
        //{
        //    if (i % 5000 == 0)
        //    {
        //        Thread.Sleep(1000);
        //    }
        //    logger.LogInformation("info hello{i} Cookie:Hm_lvt_363cefd500141425929a7561e644cdf8=1675747407; Webstorm-13df1ae=b87d0832-90d2-4b32-af4a-89fed0c9a6da; grafana_session=9410cae16aef00e34f4fb5411d986114; grafana_session_expiry=1688538963; m=59b9:true", i);
        //}
        //Console.WriteLine($"记录第{j}次100w数据了");
        
        logger.LogInformation(GenerateRandomString(20), j);

    }

}

string GenerateRandomString(int length)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();

    var result = new char[length];
    for (int i = 0; i < length; i++)
    {
        result[i] = chars[random.Next(chars.Length)];
    }

    return new string(result);
}


Console.WriteLine("end log");


Console.ReadKey();