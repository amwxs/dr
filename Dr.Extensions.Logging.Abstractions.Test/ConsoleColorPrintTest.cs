namespace Dr.Extensions.Logging.Abstractions.Test;

public class ConsoleColorPrintTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void ConsoleOut_Should_Contains_StructLog(int logLevel)
    {        // 准备测试数据
        // 准备测试数据
        var structLog = new StructLog { LogLevel = logLevel };

        var consoleColorPrint = new ConsoleColorPrint();
        // 重置控制台状态
        Console.ResetColor();
        // 重置控制台输出流
        var writer = new StringWriter();
        Console.SetOut(writer);

        // 调用被测试的方法或逻辑
        consoleColorPrint.Print(structLog);
        // 获取控制台输出结果
        var consoleOutput = writer.ToString().TrimEnd();

        // 验证预期的输出结果
        Assert.Contains(structLog.ToIndentedJson(), consoleOutput);
    }
}