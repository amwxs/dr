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
        var search = new SearchRequest("drlogs")
        {
            Query = new BoolQuery
            {
                Must = q
            }
        };
        var res = await client.SearchAsync<EntireLog>(search, cancellationToken);
        if (!res.IsValid)
        {
            var errorReason = res.OriginalException?.Message ?? res.ServerError?.ToString();
            return CustResult.Failure<EntireLog>("4000", errorReason ?? string.Empty, null);
        }
        var hit = res.Hits.FirstOrDefault();
        if (hit == null)
        {
            return CustResult.Failure<EntireLog>("4000", "not found", null);
        }

        var log = hit.Source;
        log.Id = hit.Id;
        return CustResult.Success(log);

    }
}
