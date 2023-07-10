using Microsoft.AspNetCore.Builder;

namespace Dr.Logging.AspNetCore;
public static class DependencyInjection
{
    public static void UseTrace(this IApplicationBuilder app)
    {
        app.UseMiddleware<TraceMiddleware>();
    }
}
