using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SyncVsAsync.AspNetCoreService
{
    public static class Program
    {
        public static void Main() =>
            CreateWebHostBuilder().Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                   .UseStartup<CompositionRoot>();
    }
}