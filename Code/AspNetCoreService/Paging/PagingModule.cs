using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreService.Paging
{
    public static class PagingModule
    {
        public static IServiceCollection AddPagingModule(this IServiceCollection services) =>
            services.AddSingleton<PageDtoValidator>();
    }
}