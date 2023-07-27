using Dr.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace Dr.Logging.AspNetCore;
public class TraceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IEnhancerAccessor _enhancerAccessor;
    private readonly ILogger<TraceMiddleware> _logger;

    public TraceMiddleware(RequestDelegate next, IEnhancerAccessor enhancerAccessor, ILogger<TraceMiddleware> logger)
    {
        _next = next;
        _enhancerAccessor = enhancerAccessor;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {

        using var enhancer = _enhancerAccessor.Create();
        var enhancerTrace = BulidEnhancerTrace(context);
        enhancer.TryAdd(EnhancerConst.EnhancerTrace, enhancerTrace);

        var log = new AspNetCoreTraceStructLog
        {
            TraceId = enhancerTrace.TraceId,
            SpanId = enhancerTrace.SpanId,
            ParentSpanId = enhancerTrace.ParentSpanId
        };

        try
        {

            log.Request.Path = context.Request.Path + context.Request.QueryString.Value;
            log.Request.Method = context.Request.Method;
            log.Request.Headers = ConvertHeaderToStringList(context.Request.Headers);
            log.Request.Body = await ReadRequestBodyAsync(context.Request);


            //响应流
            using var currentStream = new MemoryStream();
            var originalBody = context.Response.Body;
            context.Response.Body = currentStream;

            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            log.Response.StatusCode = context.Response.StatusCode;
            log.Response.Headers = ConvertHeaderToStringList(context.Response.Headers);
            log.Response.Body = await ReadResponseBodyAsync(context.Response);
            log.Elapsed = stopwatch.ElapsedMilliseconds;
            await currentStream.CopyToAsync(originalBody);

        }
        catch (Exception ex)
        {
            log.Exception = ex.ToString();

            throw;
        }
        finally
        {
            _logger.HttpTrace(log);
        }

    }

    private static List<string> ConvertHeaderToStringList(IHeaderDictionary headers)
    {
        var list = new List<string>();
        foreach (var header in headers)
        {
            list.Add($"{header.Key}:{header.Value}");
        }
        return list;
    }

    private static LogEnhancer BulidEnhancerTrace(HttpContext context)
    {
        var enhancer = new LogEnhancer();
        if (!context.Request.Headers.TryGetValue(EnhancerConst.TraceId, out var traceId) || string.IsNullOrEmpty(traceId))
        {
            traceId = Guid.NewGuid().ToString("N");
        }
       

        if (!context.Request.Headers.TryGetValue(EnhancerConst.SpanId, out var spanId) || string.IsNullOrEmpty(spanId))
        {
            spanId = Guid.NewGuid().ToString("N");
        }
        else
        {
            enhancer.ParentSpanId = spanId;

            spanId = Guid.NewGuid().ToString("N");;
        }
        enhancer.TraceId = traceId;
        enhancer.SpanId = spanId;
        return enhancer;
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        // 读取请求体数据
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return requestBody;
    }

    private static async Task<string> ReadResponseBodyAsync(HttpResponse response)
    {
        // 复制响应流
        response.Body.Position = 0;
        using var reader = new StreamReader(response.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);
        var responseBody = await reader.ReadToEndAsync();
        return responseBody;
    }

}