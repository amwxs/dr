using Dr.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
namespace Dr.Extensions.Logging.Kafka;

public static class DependencyInjection
{
    public static ILoggingBuilder KafkaSink(this ILoggingBuilder builder)
    {
        builder.Services.TryAddSingleton<IKafkaClient, KafkaClient>();
        builder.Services.RemoveAll<ISink>();
        builder.Services.AddSingleton<ISink, KafkaSink>();

        return builder;
    }

    public static ILoggingBuilder KafkaSink(this ILoggingBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<KafkaOptions>(section);
        builder.KafkaSink();
        return builder;
    }

    public static ILoggingBuilder KafkaSink(this ILoggingBuilder builder, Action<KafkaOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.KafkaSink();
        return builder;
    }
}
