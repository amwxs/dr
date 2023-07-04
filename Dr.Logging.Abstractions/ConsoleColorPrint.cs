namespace Dr.Logging.Abstractions;
public class ConsoleColorPrint : IConsoleColorPrint
{
    public void Print(StructLog structLog)
    {
        var color = ConsoleColor.DarkGreen;
        switch (structLog.LogLevel)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                color = ConsoleColor.Yellow;
                break;
            case 4:
                color = ConsoleColor.Red;
                break;
            case 5:
                color = ConsoleColor.Red;
                break;
            default:
                break;
        }
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("===================================================================================");
        Console.ForegroundColor = color;
        Console.WriteLine(structLog.ToIndentedJson());
        Console.ForegroundColor = ConsoleColor.DarkGreen;
    }
}
