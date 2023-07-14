using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.Abstractions.Test;
public class LoggerProviderTest
{

    [Fact]
    public void Factory_CraeteLogger_Should_ReturnLogger()
    {
        var factory = LoggerFactory.Create(c => c.AddDrLogger(o =>
        {
            o.IsConsolePrint = true;
            o.LogLevel.Add("Default", LogLevel.Information);
        }));
        var log = factory.CreateLogger("LoggerProviderTest");
        Assert.NotNull(log);
    }

    [Fact]
    public void Factory_CreateSameNameTwice_Logger_ShouldEqual()
    {
        var factory = LoggerFactory.Create(c => c.AddDrLogger(o =>
        {
            o.IsConsolePrint = true;
            o.LogLevel.Add("Default", LogLevel.Information);
        }));
        var log1 = factory.CreateLogger("LoggerProviderTest");
        var log2 = factory.CreateLogger("LoggerProviderTest");
        Assert.Equal(log1, log2);
    }


    [Fact]
    public void LoggerProvider_CreateLogger_Should_ReturnLogger()
    {
        var mockLogLevelFilter = new Mock<ILogLevelFilter>();
        var mockStructLogProcessor = new Mock<IStructLogProcessor>();
        var mokcStructLogBuilder = new Mock<IStructLogBuilder>();
        var loggerProvider = new LoggerProvider(mockLogLevelFilter.Object, mockStructLogProcessor.Object, mokcStructLogBuilder.Object);

        var categoryName = "CreateLogger";
        var log = loggerProvider.CreateLogger(categoryName);
        Assert.NotNull(log);
    }

    [Fact]
    public void CreateSameNameTwice_Logger_Should_Equal()
    {
        var mockLogLevelFilter = new Mock<ILogLevelFilter>();
        var mockStructLogProcessor = new Mock<IStructLogProcessor>();
        var mokcStructLogBuilder = new Mock<IStructLogBuilder>();
        var loggerProvider = new LoggerProvider(mockLogLevelFilter.Object, mockStructLogProcessor.Object, mokcStructLogBuilder.Object);

        var categoryName = "CreateLogger";
        var log1 = loggerProvider.CreateLogger(categoryName);
        var log2 = loggerProvider.CreateLogger(categoryName);
        Assert.Equal(log1, log2);
    }

    [Fact]
    public void LoggerProvider_Release_Should_CallDispose()
    {
        var mockLogLevelFilter = new Mock<ILogLevelFilter>();
        var mockStructLogProcessor = new Mock<IStructLogProcessor>();
        var mokcStructLogBuilder = new Mock<IStructLogBuilder>();
        var loggerProvider = new LoggerProvider(mockLogLevelFilter.Object, mockStructLogProcessor.Object, mokcStructLogBuilder.Object);

        loggerProvider.Dispose();
    }
}
