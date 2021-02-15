using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SyncVsAsync.AspNetCoreService
{
    public sealed class CompositionRoot
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting()
               .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}