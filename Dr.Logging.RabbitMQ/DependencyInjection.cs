using Dr.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Dr.Logging.RabbitMQ;
public static class DependencyInjection
{
    public static IServiceCollection RabbitMQSink(this IServiceCollection services)
    {
        services.RemoveAll<ILogSink>();
        services.AddSingleton<ILogSink, RabbitMQSink>();
        return services;
    }

    public static ILoggingBuilder RabbitMQSink(this ILoggingBuilder builder)
    {
        builder.Services.RabbitMQSink();
        return builder;
    }
}
