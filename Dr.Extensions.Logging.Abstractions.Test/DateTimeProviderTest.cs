namespace Dr.Extensions.Logging.Abstractions.Test;
public class DateTimeProviderTest
{
    [Fact]
    public void Current_Should_Equal_DateTimeNow()
    {
        var dp = new DateTimeProvider();
        var current = dp.Current.ToShortTimeString();
        var dateNow = DateTime.Now.ToShortTimeString(); ;
        Assert.Equal(current, dateNow);
    }

}
