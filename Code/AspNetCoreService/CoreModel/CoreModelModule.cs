using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreService.CoreModel
{
    public static class CoreModelModule
    {
        public static IServiceCollection AddCoreModelModule(this IServiceCollection services) =>
            services.AddSingleton<ContactValidator>();
    }
}