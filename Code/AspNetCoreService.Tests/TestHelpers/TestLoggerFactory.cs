using Microsoft.Extensions.Logging;
using Serilog;
using Xunit.Abstractions;

namespace AspNetCoreService.Tests.TestHelpers
{
    public static class TestLoggerFactory
    {
        public static ILoggerFactory CreateLoggerFactory(this ITestOutputHelper output)
        {
            var serilogLogger = output.CreateTestLogger();
            return LoggerFactory.Create(builder => builder.AddSerilog(serilogLogger));
        }
    }
}