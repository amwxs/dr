using Nest;

namespace Dr.Management.Data;
public interface IElsticSearchFactory
{
    ElasticClient Create();
}