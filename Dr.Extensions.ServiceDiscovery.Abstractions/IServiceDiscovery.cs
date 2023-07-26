namespace Dr.Extensions.ServiceDiscovery.Abstractions;
public interface IServiceDiscovery
{
    List<ServiceEntry> GetServiceAsync(string serviceName,params string[] tag);
}
