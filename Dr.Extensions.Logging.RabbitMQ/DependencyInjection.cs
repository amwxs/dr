using Dr.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Dr.Extensions.Logging.RabbitMQ;
public static class DependencyInjection
{
    public static ILoggingBuilder AddRabbitMQSink(this ILoggingBuilder builder)
    {
        builder.Services.TryAddSingleton<IMQClient, MQClient>();
        builder.Services.RemoveAll<ISink>();
        builder.Services.AddSingleton<ISink, RabbitMQSink>();
        return builder;
    }

    public static ILoggingBuilder AddRabbitMQSink(this ILoggingBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<RabbitMQOptions>(section);
        builder.AddRabbitMQSink();
        return builder;
    }

    public static ILoggingBuilder AddRabbitMQSink(this ILoggingBuilder builder, Action<RabbitMQOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.AddRabbitMQSink();
        return builder;
    }
}
