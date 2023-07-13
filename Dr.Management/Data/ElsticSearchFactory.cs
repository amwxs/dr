using Dr.Management.Core.Entities;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;

namespace Dr.Management.Data;

public class ElsticSearchFactory : IElsticSearchFactory
{
    private readonly ElasticOptioins _elasticOptioins;
    private ElasticClient? _elasticClient;

    public ElsticSearchFactory(IOptionsMonitor<ElasticOptioins> options)
    {
        _elasticOptioins = options.CurrentValue;
    }

    public ElasticClient Create()
    {
        if (_elasticClient != null)
        {
            return _elasticClient;
        }
        lock (this)
        {
            if (_elasticClient == null)
            {
                var pool = new SingleNodeConnectionPool(new Uri(_elasticOptioins.Url));

                var settings = new ConnectionSettings(pool);
                _elasticClient = new ElasticClient(settings);
            }
        }
        return _elasticClient;
    }
}
