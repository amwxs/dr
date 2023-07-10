using Confluent.Kafka;
using Dr.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Dr.Extensions.Logging.Kafka;
public class KafkaClient : IKafkaClient, IDisposable
{
    private  IProducer<Null, string> _producer;
    private  KafkaOptions _kafkaOptions;
    private readonly IDisposable? _onChangeToken;
    private readonly ILocalFileWriter _localFileWriter;
    public KafkaClient(IOptionsMonitor<KafkaOptions> options, ILocalFileWriter localFileWriter)
    {
        _localFileWriter = localFileWriter;
        _kafkaOptions = options.CurrentValue;
        _onChangeToken = options.OnChange(c => _kafkaOptions = c);
        _producer = Connent();
       
    }
    private IProducer<Null, string> Connent()
    {
        try
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                SaslMechanism = _kafkaOptions.SaslMechanism,
                SaslUsername = _kafkaOptions.SaslUsername,
                SaslPassword = _kafkaOptions.SaslPassword,
                SecurityProtocol = _kafkaOptions.SecurityProtocol,
                SslCaLocation = _kafkaOptions.SslCaLocation,
                SslCertificateLocation = _kafkaOptions.SslCertificateLocation,
                SslKeyLocation = _kafkaOptions.SslKeyLocation
            };
            return new ProducerBuilder<Null, string>(config).Build();
        }
        catch (Exception ex)
        {
            _localFileWriter.Log(new LocalFileMessage { Message = ex.ToString() });
            throw;
        }

    }

    public void BasicPublish(List<StructLog> structLogs)
    {
        foreach (var item in structLogs)
        {
            _producer.Produce(_kafkaOptions.Topic, new Message<Null, string> { Value = item.ToJson() }, deliveryReport =>
            {
                if (deliveryReport.Error.IsError)
                {
                   
                    _localFileWriter.Log(new LocalFileMessage {Message = deliveryReport.Error.Reason });
                    _localFileWriter.Log(new LocalFileMessage { FileName = "log", Message = deliveryReport.Value });
                }
            });
        }
    }

    public void Dispose()
    {
        _onChangeToken?.Dispose();
    }
}
