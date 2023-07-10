using Dr.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.RabbitMQ;
public static class DependencyInjection
{
    public static ILoggingBuilder RabbitMQSink(this ILoggingBuilder builder)
    {
        builder.Services.TryAddSingleton<IMQClient, MQClient>();
        builder.Services.RemoveAll<ILogSink>();
        builder.Services.AddSingleton<ILogSink, RabbitMQSink>();
        return builder;
    }

    public static ILoggingBuilder RabbitMQSink(this ILoggingBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<RabbitMQOptions>(section);
        builder.RabbitMQSink();
        return builder;
    }

    public static ILoggingBuilder RabbitMQSink(this ILoggingBuilder builder, Action<RabbitMQOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.RabbitMQSink();
        return builder;
    }
}
