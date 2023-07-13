using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;

namespace Dr.Management.Data;

public class ElsticSearchFactory : IElsticSearchFactory
{
    private readonly ElasticOptioins _elasticOptioins;
    private ElasticClient? _elasticClient;
    public ElsticSearchFactory(IOptions<ElasticOptioins> options)
    {
        _elasticOptioins = options.Value;
    }

    public ElasticClient Create()
    {
        if (_elasticClient != null)
        {
            return _elasticClient;
        }

        if (string.IsNullOrEmpty(_elasticOptioins.Url))
        {
            throw new ArgumentNullException("Elasticsearch url is null or empty!");
        }
        lock (this)
        {
            if (_elasticClient == null)
            {
                var pool = new SingleNodeConnectionPool(new Uri(_elasticOptioins.Url));

                var settings = new ConnectionSettings(pool);
                settings.BasicAuthentication(_elasticOptioins.UserName, _elasticOptioins.Password);
                _elasticClient = new ElasticClient(settings);
            }
        }
        return _elasticClient;
    }
}
