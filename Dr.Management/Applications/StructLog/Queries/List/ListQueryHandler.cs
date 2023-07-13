using Dr.Management.Core;
using Dr.Management.Core.Entities;
using Dr.Management.Data;
using MediatR;
using Nest;

namespace Dr.Management.Applications.StructLog.Queries.List;

public class ListQueryHandler : IRequestHandler<ListQuery, CustResult<List<BaseLog>>>
{
    private readonly IElsticSearchFactory _elsticSearchFactory;

    public ListQueryHandler(IElsticSearchFactory elsticSearchFactory)
    {
        _elsticSearchFactory = elsticSearchFactory;
    }

    public async Task<CustResult<List<BaseLog>>> Handle(ListQuery request, CancellationToken cancellationToken)
    {
        var client = _elsticSearchFactory.Create();

        var q = new List<QueryContainer>();

        #region Query Conditions


        if (!string.IsNullOrEmpty(request.HostIp))
        {
            q.Add(new TermQuery { Field = "HostIp", Value = request.HostIp });
        }
        if (!string.IsNullOrEmpty(request.AppId))
        {
            q.Add(new TermQuery { Field = "AppId", Value = request.AppId });
        }
        if (!string.IsNullOrEmpty(request.TraceId))
        {
            q.Add(new TermQuery { Field = "TraceId", Value = request.TraceId });
        }
        if (!string.IsNullOrEmpty(request.SpanId))
        {
            q.Add(new TermQuery { Field = "SpanId", Value = request.SpanId });
        }
        if (!string.IsNullOrEmpty(request.ParentSpanId))
        {
            q.Add(new TermQuery { Field = "ParentSpanId", Value = request.ParentSpanId });
        }
        if (request.LogLevel != -999)
        {
            q.Add(new TermQuery { Field = "LogLevel", Value = request.LogLevel });
        }
        if (request.EventId != -999)
        {
            q.Add(new TermQuery { Field = "EventId", Value = request.EventId });
        }

        if (!string.IsNullOrEmpty(request.Message))
        {
            q.Add(new MatchQuery { Field = "Message", Query = request.Message });
        }

        if (!string.IsNullOrEmpty(request.Exception))
        {
            q.Add(new MatchQuery { Field = "Exception", Query = request.Exception });
        }
        if (request.StartCreateTime.HasValue && request.EndCreateTime.HasValue)
        {
            q.Add(new DateRangeQuery { Field = "CreateTime", GreaterThan = request.StartCreateTime.Value, LessThan = request.EndCreateTime.Value });
        }

        if (!string.IsNullOrEmpty(request.RequestPath))
        {
            q.Add(new MatchQuery { Field = "Request.Path", Query = request.RequestPath });
        }
        if (!string.IsNullOrEmpty(request.RequestMethod))
        {
            q.Add(new TermQuery { Field = "Request.Method", Value = request.RequestMethod });
        }
        if (!string.IsNullOrEmpty(request.RequestBody))
        {
            q.Add(new MatchQuery { Field = "Request.Body", Query = request.RequestBody });
        }
        if (!string.IsNullOrEmpty(request.RequestHeader))
        {
            q.Add(new MatchQuery { Field = "Request.Headers", Query = request.RequestHeader });
        }

        if (request.ResponseStatusCode != -999)
        {
            q.Add(new TermQuery { Field = "Response.StatusCode", Value = request.ResponseStatusCode });
        }
        if (!string.IsNullOrEmpty(request.ResponseBody))
        {
            q.Add(new MatchQuery { Field = "Response.Body", Query = request.RequestBody });
        }
        if (!string.IsNullOrEmpty(request.ResponseHeader))
        {
            q.Add(new MatchQuery { Field = "Response.Headers", Query = request.RequestHeader });
        }
        #endregion

        var search = new SearchRequest("drlogs-*")
        {

            Query = new BoolQuery
            {
                Must = q
            },
            From = (request.PageIndex - 1) * request.PageSize,
            Size = request.PageSize,
            Source = new SourceFilter
            {
                Excludes = new[] { "@timestamp", "@version", "Response", "Request", "Message", "Exception" }
            }
        };

        var res = await client.SearchAsync<BaseLog>(search, cancellationToken);
        if (!res.IsValid)
        {
            var errorReason = res.OriginalException?.Message ?? res.ServerError?.ToString() ?? string.Empty;
            return CustResult.Failure<List<BaseLog>>("4000", errorReason, null);
        }

        var logs = new List<BaseLog>();
        foreach (var hit in res.Hits)
        {
            var log = hit.Source;
            log.Id = hit.Id;
            logs.Add(log);
        }
        return CustResult.Success(logs, new Pager(request.PageIndex, request.PageSize, res.Total));

    }
}
