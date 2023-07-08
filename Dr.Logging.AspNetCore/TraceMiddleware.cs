using Dr.Logging.Abstractions;
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

        var log = new HttpTraceStructLog
        {
            TraceId = enhancerTrace.TraceId,
            SpanId = enhancerTrace.SpanId,
            ParentSpanId = enhancerTrace.ParentSpanId
        };

        try
        {

            log.Request.Path = context.Request.Path;
            log.Request.Body = await ReadRequestBodyAsync(context.Request);
            log.Request.Headers = context.Request.Headers;

            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            log.ElapsedMillisecond = stopwatch.ElapsedMilliseconds;

            log.Response.Body = await ReadResponseBodyAsync(context.Response);
            log.Response.StatusCode = context.Response.StatusCode;
            log.Response.Headers = context.Response.Headers;
            _logger.HttpTrace(log);
        }
        catch (Exception ex)
        {
            log.Exception = ex.ToString();
            _logger.HttpTrace(log);
            throw;
        }
    }

    private static LogEnhancer BulidEnhancerTrace(HttpContext context)
    {
        var enhancer = new LogEnhancer();
;
        if (!context.Request.Headers.TryGetValue(EnhancerConst.TraceId, out var traceId) || string.IsNullOrEmpty(traceId))
        {
            traceId = Guid.NewGuid().ToString("N");
            context.Request.Headers.TryAdd(EnhancerConst.TraceId, traceId);
        }
       

        if (!context.Request.Headers.TryGetValue(EnhancerConst.SpanId, out var spanId) || string.IsNullOrEmpty(spanId))
        {
            spanId = Guid.NewGuid().ToString("N");
            context.Request.Headers.TryAdd(EnhancerConst.SpanId, spanId);
        }
        else
        {
            enhancer.ParentSpanId = spanId;

            spanId = Guid.NewGuid().ToString("N");
            context.Request.Headers.Remove(EnhancerConst.SpanId);
            context.Request.Headers.TryAdd(EnhancerConst.SpanId, spanId);
        }
        enhancer.TraceId = traceId;
        enhancer.SpanId = spanId;
        return enhancer;
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        // 读取请求体数据
        using var reader = new StreamReader(request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return requestBody;
    }

    private static async Task<string> ReadResponseBodyAsync(HttpResponse response)
    {
        // 复制响应流
        using var responseStream = new MemoryStream();
        var originalBody = response.Body;
        await response.Body.CopyToAsync(responseStream);
        responseStream.Seek(0, SeekOrigin.Begin);
        response.Body = originalBody;

        // 读取响应体数据
        string responseBody = await new StreamReader(responseStream).ReadToEndAsync();
        responseStream.Seek(0, SeekOrigin.Begin);

        return responseBody;
    }

}