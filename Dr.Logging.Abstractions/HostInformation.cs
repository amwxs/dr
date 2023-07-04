using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Dr.Logging.Abstractions;
public class HostInformation : IHostInformation
{
    private static string _hostIP = string.Empty;

    public string HostIp()
    {
        if (!string.IsNullOrEmpty(_hostIP))
        {
            return _hostIP;
        }
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        var ipAddresses = networkInterfaces
            .Where(ni => ni.NetworkInterfaceType != NetworkInterfaceType.Loopback && !ni.Description.ToLower().Contains("virtual"))
            .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
            .Where(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork)
            .Select(ua => ua.Address.ToString())
            .ToList();
        if (ipAddresses.Count > 0)
        {
            _hostIP = ipAddresses[0];

        }
        return _hostIP;
    }
}
