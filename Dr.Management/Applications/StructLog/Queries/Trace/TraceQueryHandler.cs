using Dr.Management.Core;
using Dr.Management.Core.Entities;
using Dr.Management.Data;
using MediatR;
using Nest;

namespace Dr.Management.Applications.StructLog.Queries.Trace;

public class TraceQueryHandler : IRequestHandler<TraceQuery, CustResult<List<TreeLog>>>
{
    private readonly IElsticSearchFactory _elsticSearchFactory;

    public TraceQueryHandler(IElsticSearchFactory elsticSearchFactory)
    {
        _elsticSearchFactory = elsticSearchFactory;
    }

    public async Task<CustResult<List<TreeLog>>> Handle(TraceQuery request, CancellationToken cancellationToken)
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
            Size = int.MaxValue
        };
        var res = await client.SearchAsync<TreeLog>(searchRequest, cancellationToken);
        if (!res.IsValid)
        {
            var errorReason = res.OriginalException?.Message ?? res.ServerError?.ToString();
            return CustResult.Failure<List<TreeLog>>("4000", errorReason ?? string.Empty, null);
        }
        var logs = res.Documents.OrderBy(x => x.CreateTime).ToList();
        //找出ParentId为空的
        var rootLogs = logs.Where(x => string.IsNullOrEmpty(x.ParentSpanId)).ToList();
        foreach (var log in rootLogs)
        {
            //查找子元素
            BuildTree(log, logs);
        }
        return CustResult.Success(rootLogs);
    }

    private void BuildTree(TreeLog parentLog, List<TreeLog> logs)
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
        parentLog.SubLogs = sublogs.ToList();

        foreach (var log in sublogs)
        {
            BuildTree(log, logs);
        }
    }
}
