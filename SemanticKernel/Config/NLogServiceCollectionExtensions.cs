using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using NLog.Targets;

public static IServiceCollection AddNLogging(this IServiceCollection services)
{
    // 定义文件日志输出目标
    var fileTarget = new FileTarget()
    {
        FileName = "sk-demo.log", AutoFlush = true, DeleteOldFileOnStartup = true
    };
    // 定义控制台日志输出目标
    var consoleTarget = new ConsoleTarget();
    var config = new NLog.Config.LoggingConfiguration();
    // 定义日志输出规则(输出所有Trace级别及以上的日志到控制台)
    config.AddRule(
        NLog.LogLevel.Trace,
        NLog.LogLevel.Fatal,
        target: fileTarget,  // 这里采用文件输出
        "*");// * 表示所有Logger
    // 注册NLog
    services.AddLogging(loggingBuilder => loggingBuilder.AddNLog(config));
    return services;
}