using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Dr.Extensions.Logging.Abstractions;
public static class DependencyInjection
{
    public static ILoggingBuilder AddDrLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.TryAddSingleton<ILogSink, NullLogSink>();
        builder.Services.TryAddSingleton<IConsoleColorPrint, ConsoleColorPrint>();
        builder.Services.TryAddSingleton<ILocalFileWriter, LocalFileWriter>();
        builder.Services.TryAddSingleton<IEnhancerAccessor, EnhancerAccessor>();
        builder.Services.TryAddSingleton<ILogLevelFilter, LogLevelFilter>();
        builder.Services.TryAddSingleton<IStructLogBuilder, StructLogBuilder>();
        builder.Services.TryAddSingleton<IStructLogProcessor, StructLogProcessor>();

        builder.Services.TryAddSingleton<IHostInformation, HostInformation>();

        LoggerProviderOptions.RegisterProviderOptions<LoggerOptions, LoggerProvider>(builder.Services);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>());

        return builder;
    }

    public static ILoggingBuilder AddDrLogger(this ILoggingBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<LoggerOptions>(section);
        builder.AddDrLogger();
        return builder;
    }

    public static ILoggingBuilder AddDrLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.AddDrLogger();
        return builder;
    }
}
