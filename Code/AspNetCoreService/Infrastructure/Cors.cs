using Light.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreService.Infrastructure
{
    public static class Cors
    {
        public static IServiceCollection AddCorsIfNecessary(this IServiceCollection services, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
                services.AddCors();
            return services;
        }

        public static IApplicationBuilder UseCorsIfNecessary(this IApplicationBuilder app, IConfiguration configuration)
        {
            var allowedCorsOrigins = configuration.GetSection("allowedCorsOrigins").Get<string[]>();
            if (allowedCorsOrigins.IsNullOrEmpty())
                return app;

            return app.UseCors(builder => builder.WithOrigins(allowedCorsOrigins)
                                          .AllowAnyHeader()
                                          .AllowAnyMethod());
        }
    }
}