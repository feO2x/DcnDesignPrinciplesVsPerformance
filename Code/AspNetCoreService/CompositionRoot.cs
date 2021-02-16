using AspNetCoreService.ContactsWebApi;
using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;
using AspNetCoreService.Infrastructure;
using AspNetCoreService.Paging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AspNetCoreService
{
    public sealed class CompositionRoot
    {
        public CompositionRoot(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddControllersAsServices();
            services.AddCoreModelModule()
                    .AddCorsIfNecessary(Environment)
                    .AddDataAccessModule(Configuration)
                    .AddPagingModule()
                    .AddContactsWebApiModule()
                    .AddAutoMapper(typeof(CompositionRoot));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                   .UseCorsIfNecessary(Configuration);
            }

            app.UseSerilogRequestLogging()
               .UseRouting()
               .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}