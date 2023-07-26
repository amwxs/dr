using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Dr.Extensions.ServiceDiscovery.Abstractions;

public class HostInfomation : IHostInfomation
{

    private readonly IServer _server;
    public HostInfomation(IServer server)
    {
        _server = server;
    }

    public string GetHostIp()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        var ipAddresses = networkInterfaces
            .Where(ni => ni.NetworkInterfaceType != NetworkInterfaceType.Loopback && !ni.Description.ToLower().Contains("virtual"))
            .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
            .Where(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork)
            .Select(ua => ua.Address.ToString())
            .ToList();
        if (ipAddresses.Count > 0)
        {
            return ipAddresses[0];

        }
        throw new ArgumentNullException("Not Find host IP!");
    }

    public int GetPort()
    {
        var address = _server.Features.Get<IServerAddressesFeature>()?.Addresses.Select(p => new Uri(p));
        if (address == null || !address.Any())
        {
            throw new ArgumentNullException("Not Find Run Port!");
        }
        return address.First().Port;
    }

}
