using Dr.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Dr.Logging.RabbitMQ;
public static class DependencyInjection
{
    public static ILoggingBuilder RabbitMQSink(this ILoggingBuilder builder)
    {
        builder.Services.TryAddSingleton<IMQClient, MQClient>();
        builder.Services.RemoveAll<ILogSink>();
        builder.Services.AddSingleton<ILogSink, RabbitMQSink>();
        return builder;
    }

    public static ILoggingBuilder RabbitMQSink(this ILoggingBuilder builder, Action<RabbitMQOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.RabbitMQSink();
        return builder;
    }
}
