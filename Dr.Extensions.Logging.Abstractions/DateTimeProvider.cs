namespace Dr.Extensions.Logging.Abstractions;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Current => DateTime.Now;
}
