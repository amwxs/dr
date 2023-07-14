using Microsoft.Extensions.Options;

namespace Dr.Extensions.Logging.Abstractions.Test;
public class LocalFileWriterTest
{
    [Fact]
    public async Task Log_WritesToCorrectFile()
    {

        var mockIOptionsMonitor = new Mock<IOptionsMonitor<LoggerOptions>>();
        mockIOptionsMonitor.Setup(x => x.CurrentValue).Returns(new LoggerOptions {LocalPath = "Logs" });

        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        dateTimeProviderMock.Setup(d => d.Current).Returns(new DateTime(2023, 7, 14));

        var writer = new LocalFileWriter(mockIOptionsMonitor.Object, dateTimeProviderMock.Object);

        var logMessage = new LocalFileMessage { FileName = "TestLog", Message = "Test message" };

        var expectedFilePath = Path.Combine("Logs", "TestLog2023-07-14.log");

        await writer.Log(logMessage);

        Assert.True(File.Exists(expectedFilePath));

        var content = File.ReadAllText(expectedFilePath);
        Assert.Contains($"datetime: {new DateTime(2023, 7, 14)}", content);
        Assert.Contains("Message: Test message", content);

        File.Delete(expectedFilePath);
    }

    [Fact]
    public async Task Log_CreatesDirectoryIfNotExists()
    {

        var mockIOptionsMonitor = new Mock<IOptionsMonitor<LoggerOptions>>();
        mockIOptionsMonitor.Setup(x => x.CurrentValue).Returns(new LoggerOptions { LocalPath = "Logs/TestDirectory" });


        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        dateTimeProviderMock.Setup(d => d.Current).Returns(new DateTime(2023, 7, 14));
        var writer = new LocalFileWriter(mockIOptionsMonitor.Object, dateTimeProviderMock.Object);

        var logMessage = new LocalFileMessage { FileName = "TestLog", Message = "Test message" };

        var expectedDirectoryPath = Path.Combine("Logs", "TestDirectory");
        var expectedFilePath = Path.Combine(expectedDirectoryPath, "TestLog2023-07-14.log");


        await writer.Log(logMessage);


        Assert.True(Directory.Exists(expectedDirectoryPath));
        Assert.True(File.Exists(expectedFilePath));


        File.Delete(expectedFilePath);
        Directory.Delete(expectedDirectoryPath);
    }
}
