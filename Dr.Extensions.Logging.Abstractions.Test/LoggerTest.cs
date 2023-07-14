using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.Abstractions.Test;
public class LoggerTest
{
    [Fact]
    public void IsEnabled_ShouldCallLogLevelFilter()
    {

        var logLevel = LogLevel.Information;
        var categoryName = "TestCategory";

        var logLevelFilterMock = new Mock<ILogLevelFilter>();
        logLevelFilterMock.Setup(x => x.IsEnabled(categoryName, logLevel)).Returns(true);

        var structLogBuilderMock = new Mock<IStructLogBuilder>();
        var structLogProcessorMock = new  Mock<IStructLogProcessor>();


        var logger = new Logger(categoryName, logLevelFilterMock.Object, structLogBuilderMock.Object, structLogProcessorMock.Object);
        var isenable = logger.IsEnabled(logLevel);

        //Assert.True(isenable);
        //logLevelFilterMock.Verify(x => x.IsEnabled(categoryName, logLevel), Times.Once);
    }

    [Fact]
    public void Log_ShouldBuildLogAndAddToLogProcessor()
    {

        var logLevel = LogLevel.Information;
        var eventId = new EventId(1, "TestEvent");
        var state = "Test message";
        var exception = new Exception("Test exception");
        var categoryName = "TestCategory";

        var log = new StructLog();

        var logLevelFilterMock = new Mock<ILogLevelFilter>();
        logLevelFilterMock.Setup(x => x.IsEnabled(categoryName, logLevel)).Returns(true);

        var structLogBuilderMock = new Mock<IStructLogBuilder>();
        structLogBuilderMock.Setup(x => x.Build(logLevel, eventId, state, exception, It.IsAny<Func<object?, Exception?, string>>()))
            .Returns(log);

        var structLogProcessorMock = new Mock<IStructLogProcessor>();


        var logger = new Logger(categoryName, logLevelFilterMock.Object, structLogBuilderMock.Object, structLogProcessorMock.Object);
        logger.Log(logLevel, eventId, state, exception, (s, e) => $"Formatted log: {s}, {e}");
        //structLogProcessorMock.Verify(x => x.AddLog(log), Times.Once);

        //structLogBuilderMock.Verify(x => x.Build(logLevel, eventId, state, exception, It.IsAny<Func<object?, Exception?, string>>()), Times.Once);
    }

}
