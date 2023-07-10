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
    {        // ׼����������
        // ׼����������
        var structLog = new StructLog { LogLevel = logLevel };

        var consoleColorPrint = new ConsoleColorPrint();
        // ���ÿ���̨״̬
        Console.ResetColor();
        // ���ÿ���̨�����
        var writer = new StringWriter();
        Console.SetOut(writer);

        // ���ñ����Եķ������߼�
        consoleColorPrint.Print(structLog);
        // ��ȡ����̨������
        var consoleOutput = writer.ToString().TrimEnd();

        // ��֤Ԥ�ڵ�������
        Assert.Contains(structLog.ToIndentedJson(), consoleOutput);
    }
}