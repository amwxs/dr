using Dr.Management.Core;
using Dr.Management.Core.Entities;
using Dr.Management.Data;
using MediatR;
using Nest;

namespace Dr.Management.Applications.StructLog.Queries.Trace;

public class TraceQueryHandler : IRequestHandler<TraceQuery, CustResult<List<TraceLog>>>
{
    private readonly IElsticSearchFactory _elsticSearchFactory;

    public TraceQueryHandler(IElsticSearchFactory elsticSearchFactory)
    {
        _elsticSearchFactory = elsticSearchFactory;
    }

    public async Task<CustResult<List<TraceLog>>> Handle(TraceQuery request, CancellationToken cancellationToken)
    {
        var client = _elsticSearchFactory.Create();
        var q = new List<QueryContainer>()
        {
            new TermQuery{ Field="TraceId", Value = request.TraceId}
        };
        var searchRequest = new SearchRequest
        {
            Query = new BoolQuery
            {
                Must = q
            },
            Source = new SourceFilter
            {
                Excludes = new[] { "@timestamp", "@version", "Response", "Request", "Message", "Exception" }
            }
        };
        var res = await client.SearchAsync<TraceLog>(searchRequest, cancellationToken);
        if (!res.IsValid)
        {
            var errorReason = res.OriginalException?.Message ?? res.ServerError?.ToString();
            return CustResult.Failure<List<TraceLog>>("4000", errorReason ?? string.Empty, null);
        }

        var logs = new List<TraceLog>();
        foreach (var hit in res.Hits)
        {
            var log = hit.Source;
            log.Id = hit.Id;
            logs.Add(log);
        }

        var roots = logs.Where(x => string.IsNullOrEmpty(x.ParentSpanId))
            .OrderBy(x => x.CreateTime).ToList();

        foreach (var log in roots)
        {
            //查找子元素
            BuildTree(log, logs);
        }
        return CustResult.Success(roots);
    }

    private void BuildTree(TraceLog parentLog, List<TraceLog> logs)
    {
        if (string.IsNullOrEmpty(parentLog.SpanId))
        {
            return;
        }
        var sublogs = logs.Where(x => x.ParentSpanId == parentLog.SpanId).OrderBy(x=>x.CreateTime).ToList();
        if (sublogs.Count == 0)
        {
            return;
        }
        parentLog.Subs = sublogs.ToList();

        foreach (var log in sublogs)
        {
            BuildTree(log, logs);
        }
    }
}
