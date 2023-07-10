using Confluent.Kafka;

namespace Dr.Extensions.Logging.Kafka;
public class KafkaOptions
{
    public string? BootstrapServers { get;  set; }
    public string? Topic { get;  set; }
    public string? SaslUsername { get; internal set; }
    public string? SaslPassword { get; internal set; }
    public SaslMechanism? SaslMechanism { get; internal set; }
    public SecurityProtocol? SecurityProtocol { get; internal set; }
    public string? SslCaLocation { get; internal set; }
    public string? SslCertificateLocation { get; internal set; }
    public string? SslKeyLocation { get; internal set; }
}
