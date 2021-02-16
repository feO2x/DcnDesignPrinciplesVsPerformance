using Microsoft.Extensions.Hosting;
using Serilog;

namespace AspNetCoreService.Infrastructure
{
    public class Logging
    {
        public static ILogger Logger => Log.Logger ??
                                        new LoggerConfiguration().WriteTo.Console()
                                                                 .WriteTo.File("Startup Error.log")
                                                                 .CreateLogger();

        public static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    }
}