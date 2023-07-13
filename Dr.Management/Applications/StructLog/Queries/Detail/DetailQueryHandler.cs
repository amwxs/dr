using Dr.Management.Core;
using Dr.Management.Core.Entities;
using Dr.Management.Data;
using MediatR;
using Nest;

namespace Dr.Management.Applications.StructLog.Queries.Detail;

public class DetailQueryHandler : IRequestHandler<DetailQuery, CustResult<EntireLog>>
{
    private readonly IElsticSearchFactory _elsticSearchFactory;

    public DetailQueryHandler(IElsticSearchFactory elsticSearchFactory)
    {
        _elsticSearchFactory = elsticSearchFactory;
    }

    public async Task<CustResult<EntireLog>> Handle(DetailQuery request, CancellationToken cancellationToken)
    {
        var client = _elsticSearchFactory.Create();
        var q = new List<QueryContainer>()
        {
            new TermQuery{ Field="_id", Value = request.Id}
        };
        var searchRequest = new SearchRequest
        {
            Query = new BoolQuery
            {
                Must = q
            }
        };
        var res = await client.GetAsync<EntireLog>(request.Id,c=>c.Index("drlogs-2023.07.13"));
        if (!res.IsValid)
        {
            var errorReason = res.OriginalException?.Message ?? res.ServerError?.ToString();
            return CustResult.Failure<EntireLog>("4000", errorReason ?? string.Empty, null);
        }
        var log = res.Source;
        log.Id = res.Id;
        return CustResult.Success(log);

    }
}
