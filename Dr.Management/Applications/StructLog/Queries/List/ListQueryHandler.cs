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

        if (!string.IsNullOrEmpty(request.AppId))
        {
            q.Add(new TermQuery { Field = "AppId", Value = request.AppId });
        }
        if (!string.IsNullOrEmpty(request.TraceId))
        {
            q.Add(new TermQuery { Field = "TraceId", Value = request.TraceId });
        }

        if (request.LogLevel >=0)
        {
            q.Add(new TermQuery { Field = "LogLevel", Value = request.LogLevel });
        }
        if (!string.IsNullOrEmpty(request.Message))
        {
            q.Add(new MatchQuery { Field = "Message", Query = request.Message });
        }

        if (!string.IsNullOrEmpty(request.Exception))
        {
            q.Add(new MatchQuery { Field = "Exception", Query = request.Exception });
        }

        if (request.StartTime.HasValue && request.EndTime.HasValue)
        {
            q.Add(new DateRangeQuery { Field = "CreateTime", GreaterThan = request.StartTime.Value, LessThan = request.EndTime.Value });
        }

        if (request.ElapsedRangeStart.HasValue && request.ElapsedRangeEnd.HasValue)
        {
            q.Add(new LongRangeQuery { Field = "Elapsed", GreaterThan = request.ElapsedRangeStart.Value, LessThan = request.ElapsedRangeEnd.Value });
        }

        if (!string.IsNullOrEmpty(request.RequestPath))
        {
            q.Add(new MatchQuery { Field = "Request.Path", Query = request.RequestPath });
        }

        if (!string.IsNullOrEmpty(request.RequestBody))
        {
            var bQuery = new BoolQuery
            {
                Should = new List<QueryContainer>
               {
                  new MatchQuery { Field = "Request.Body", Query = request.RequestBody },
                  new MatchQuery { Field = "Request.Headers", Query = request.RequestBody }
               }
            };
            q.Add(bQuery);
        }

        if (request.ResponseStatusCode >=0)
        {
            q.Add(new TermQuery { Field = "Response.StatusCode", Value = request.ResponseStatusCode });
        }
        if (!string.IsNullOrEmpty(request.ResponseBody))
        {
            var bQuery = new BoolQuery
            {
               Should = new List<QueryContainer>
               {
                   new MatchQuery { Field = "Response.Body", Query = request.RequestBody },
                   new MatchQuery { Field = "Response.Headers", Query = request.RequestBody }
               }
            };
            q.Add(bQuery);
        }
        #endregion

        var search = new SearchRequest<BaseLog>("drlogs")
        {

            Query = new BoolQuery
            {
                Must = q
            },
            From = (request.PageIndex - 1) * request.PageSize,
            Size = request.PageSize,
            Source = new SourceFilter
            {
                Excludes = new[] { "@timestamp", "@version", "Response", "Request" }
            },
            Sort = new List<ISort>
            {
                new FieldSort
                {
                    Field ="CreateTime",
                    Order = SortOrder.Descending
                }
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
