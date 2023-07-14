namespace Dr.Extensions.Logging.Abstractions.Test;
public class HostInformationTest
{
    [Fact]
    public void GetHostIp_Should_NotEmpty()
    {
        var hostInformation = new HostInformation();
        var ip = hostInformation.HostIp();
        Assert.NotEmpty(ip);
    }
}
