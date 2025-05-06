using GreenFlux.Application;
using GreenFlux.Infrastructure;

namespace GreenFlux.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddApplicationDI();
            services.AddInfrastructureDI(configuration, environment);
            return services;
        }
    }
}