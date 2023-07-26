namespace Dr.Extensions.ServiceDiscovery.Abstractions;
public class ServiceEntry
{
    public int Port { get; set; }
    public string HostIP { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
}
